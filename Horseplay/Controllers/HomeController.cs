using Horseplay.Classes;
using Horseplay.DAL;
using Horseplay.Models;
using Horseplay.Static;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Horseplay.ViewModels;
using System.Data.Entity.Validation;

namespace Horseplay.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            using (HorseplayContext db = new HorseplayContext())
            {
                return View(db.Users.ToList());
            }
            
        }
        public ActionResult AcceptInvitation(string id)
        {
            HorseplayContext db = new HorseplayContext();
            var invs = db.Invitations.Where(i => i.InvitationToken == id.Trim() && i.IsAccepted==false);
            if (invs.Any())
            {
                //correct token, let's add new user to that Tenant
                AddingUserViewModel vm = new AddingUserViewModel();
                int tenantId = invs.FirstOrDefault().TenantId;
                TempData["TenantId"] = tenantId;
                TempData["InvitationToken"] = id;
                User user = new Models.User() { TenantId = tenantId, Email = invs.FirstOrDefault().InvitedMail };
                vm.User = user;
                vm.Invitation = invs.FirstOrDefault();
                return View("AddUser", vm);
            }
            else
            {
                return new HttpNotFoundResult("Nie znaleziono strony");
            }
        }

        public ActionResult ConfirmUserMail(string id)
        {
            HorseplayContext db = new HorseplayContext();
            var users = db.Users.Where(u=>u.ConfirmationToken == id.Trim() && u.IsConfirmed==false);
            if (users.Any())
            {
                User user = users.FirstOrDefault();
                //correct token, let's confirm that account
                user.ConfirmationDate = DateTime.Now;
                user.IsConfirmed = true;
                user.ConfirmPassword = user.Password;//just to satisfy crazy EF validation thing
                db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                try
                {
                    db.SaveChanges();
                }catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Debug.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw;
                }

                TempData["Info"] = "Twoje konto zostało aktywowane!";
                return RedirectToAction("Login");
                
            }
            else
            {
                return new HttpNotFoundResult("Nie znaleziono strony");
            }
        }

        public ActionResult AddUser()
        {
            AddingUserViewModel vm = new AddingUserViewModel();
            vm.User = new User();
            return View("AddUser",vm);
        }

        [HttpPost]
        public ActionResult AddUser(AddingUserViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View("AddUser", vm);
            }else
            {
                HorseplayContext db = new HorseplayContext();
                if (db.Users.Any(x=>x.Login == vm.User.Login))
                {
                    //user of such login already exists, drop error
                    ModelState.AddModelError(string.Empty, "Użytkownik o takim loginie już istnieje. Podaj unikatowy login");
                    return View("AddUser", vm.User);
                }else if (db.Users.Any(x=>x.Email.Trim() == vm.User.Email.Trim()))
                {
                    //user of such e-mail already exists, drop error
                    ModelState.AddModelError(string.Empty, "Użytkownik o takim adresie e-mail już istnieje. Podaj unikatowy adres e-mail");
                    return View("AddUser", vm);
                }
                else
                {
                    //validation done, let's add user
                    
                    if (vm.User.TenantId == 0)
                    {
                        string token = Utilities.uniqueToken(db);
                        vm.User.ConfirmationToken = token.Trim();
                        vm.User.ExpirationDate = DateTime.Now.AddDays(14);
                        string result = SendVerificationToken(token, vm.User.Email);
                        TempData["Info"] = result;
                        vm.User.createdOn = DateTime.Now;
                        Tenant tenant = new Tenant { createdOn = DateTime.Now };
                        db.Tenants.Add(tenant);
                        try
                        {
                            db.SaveChanges();
                        }catch(Exception ex)
                        {
                            Debug.WriteLine(ex.Message + ", " + ex.InnerException);
                        }
                        vm.User.TenantId = tenant.TenantId;
                        db.Users.Add(vm.User);
                        try
                        {
                            db.SaveChanges();
                        }catch(Exception ex)
                        {
                            Debug.WriteLine(ex.Message + ", " + ex.InnerException);
                        }
                        tenant.createdBy = vm.User.UserId;
                        try
                        {
                            db.SaveChanges();
                        }catch(Exception ex)
                        {
                            Debug.WriteLine(ex.Message + ", " + ex.InnerException);
                        }
                    }
                    else
                    {
                        Invitation Invitation = db.Invitations.Where(i => i.InvitationToken == vm.Invitation.InvitationToken).FirstOrDefault();
                        Invitation.AcceptedOn = DateTime.Now;
                        Invitation.IsAccepted = true;
                        vm.User.ConfirmationToken = Invitation.InvitationToken;
                        vm.User.ExpirationDate = Invitation.ExpirationDate;
                        vm.User.createdOn = DateTime.Now;
                        vm.User.ConfirmationDate = DateTime.Now;
                        vm.User.IsConfirmed = true;
                        db.Entry(Invitation).State = System.Data.Entity.EntityState.Modified;
                        db.Users.Add(vm.User);
                        db.SaveChanges();

                    }

                    return View("userCreated", vm.User);
                }

            }
        }
        [NonAction]
        public ActionResult userCreated()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]  
        public ActionResult Login(User user)
        {
            using(HorseplayContext db = new HorseplayContext())
            {
                var usr = (from us in db.Users
                           where (us.Login.Trim() == user.Login.Trim() || us.Email.Trim() ==user.Login.Trim()) && us.Password.Trim() == user.Password.Trim()
                           select us
                           ).FirstOrDefault();
                if(usr != null)
                {   
                    if(!usr.IsConfirmed && usr.ExpirationDate < DateTime.Now)
                    {
                        //we've just go johny come lately
                        string token = Utilities.uniqueToken(db);
                        usr.ConfirmationToken = token.Trim();
                        var oldTerm = usr.ExpirationDate;
                        usr.ConfirmPassword = usr.Password; //just to satisfy crazy EF validation thing
                        db.Entry(usr).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        string result = SendVerificationToken(token, usr.Email);
                        TempData["Error"] = "Konto nie zostało aktywowane, pomimo upływu terminu w dniu " + oldTerm + ". Na adres " + usr.Email + " została wysłana nowa wiadomość z linkiem aktywacyjnym. Dostęp do konta zostanie przywrócony, gdy użytkownik aktywuje konto korzystając z linka umieszczonego w otrzymanej wiadomości e-mail";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        //all ok, let's log him in
                        Session["UserId"] = usr.UserId;
                        Session["Login"] = usr.Login;
                        Session["TenantId"] = usr.TenantId;
                        usr.ConfirmPassword = usr.Password; //just to satisfy crazy EF validation thing
                        usr.lastLogged = DateTime.Now;
                        db.Entry(usr).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        ModelState.Clear();
                        return RedirectToAction("Index", "Account");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Błędny login lub hasło!");
                    return View("Login", user);
                }

            }
        }

        public ActionResult userLogged()
        {
            return View("../Account/Index");
        }

        public ActionResult AboutAuthor()
        {
            return Redirect("https://rr-soft.pl");
        }

        public ActionResult AboutApplication()
        {
            return View();
        }

        public string SendVerificationToken(string token, string mail)
        {
            string result;
            try
            {
                //try send mail first
                Mail newMail = new Mail();
                string textBody = "Witaj na Systo! Tego maila otrzymujesz, ponieważ ktoś podał Twój adres e-mail jako adres kontaktowy przy tworzeniu nowego konta. Jeśli to Ty, skopiuj ten adres do przeglądarki, zatwierdź klawiszem ENTER: https://systo.rr-soft.pl/Home/ConfirmUserMail/" + token + " . Na potwierdzenie adresu masz 14 dni od daty otrzymania tej wiadomości, po upływie tego terminu niepotwierdzone konta zostaną usunięte. Jeśli to nie Ty rejestrowałeś się w usułdze Systo a Twój adres został użyty bez Twojej zgody, prosimy o zignorowanie tej wiadomości!";
                string htmlBody = "<h2 align=\"center\"Witaj na Systo!</h2><br>";
                htmlBody += "<p align=\"center\"><font size=\"4\">Tego maila otrzymujesz, ponieważ ktoś podał Twój adres e-mail jako adres kontaktowy przy tworzeniu nowego konta. Jeśli to Ty rejestrowałeś się w usłudze Systo, kliknij poniższy adres aby zakończyć proces tworzenia konta. Na potwierdzenie adresu masz 14 dni od daty otrzymania tej wiadomości, po upływie tego terminu niepotwierdzone konta zostaną usunięte.</font></p>";
                htmlBody += "<br><p align=\"center\"><a href=\"https://systo.rr-soft.pl/Home/ConfirmUserMail/" + token + "\">systo.rr-soft.pl/Home/ConfirmUserMail/" + token + "/</a></p>";
                htmlBody += "<br><br><p align=\"center\"><font size=\"2\">Jeśli to nie Ty rejestrowałeś się w usłudze Systo a Twój adres został użyty bez Twojej zgody, prosimy o zignorowanie tej wiadomości!</font></p>";
                htmlBody += "<br><br><p align=\"center\"><font size=\"2\">Wiadomość zotała wysłana automatycznie, prosimy na nią nie odpowiadać</font></p>";
                newMail.Send(mail, "Link aktywacyjny do konta w Systo", textBody, htmlBody);
                result = "Na adres " + mail + " została wysłana wiadomość z linkiem aktywacyjnym. Jeśli konto nie zostanie aktywowane w ciągu 14 dni, dostęp do niego zostanie zablokowany!";
            }
            catch (Exception ex)
            {
                result = "Wystąpiły problem podczas próby wysłania linku aktywacyjnego do " + mail + ".. " + ex.Message;
            }
            return result;
        }
    }
}