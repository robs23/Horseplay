using Horseplay.Classes;
using Horseplay.DAL;
using Horseplay.Models;
using Horseplay.Static;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Horseplay.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            if (Session["TenantId"] == null)
            {
                TempData["Info"] = "Nastąpiło automatyczne wylogowanie. Proszę ponownie się zalogować";
                return RedirectToAction("Login", "Home");
            }
            else
            {
                return View();
            }
        }

        public ActionResult GetUsers()
        {
            if (Session["TenantId"] == null)
            {
                TempData["Info"] = "Nastąpiło automatyczne wylogowanie. Proszę ponownie się zalogować";
                return RedirectToAction("Login", "Home");
            }
            else
            {
                HorseplayContext db = new HorseplayContext();
                int tenantId = (int)Session["TenantId"];
                var users = db.Users.Where(u => u.TenantId == tenantId);
                return View(users);
            }
        }

        public ActionResult GetInvitations()
        {
            if (Session["TenantId"] == null)
            {
                TempData["Info"] = "Nastąpiło automatyczne wylogowanie. Proszę ponownie się zalogować";
                return RedirectToAction("Login", "Home");
            }
            else
            {
                HorseplayContext db = new HorseplayContext();
                int tenantId = (int)Session["TenantId"];
                var invs = db.Invitations.Where(i => i.TenantId == tenantId).OrderBy(i => i.IsAccepted).ThenByDescending(i => i.DateAdded);
                return View(invs);
            }
        }

        public ActionResult InvitationDetails(int id)
        {
            if (Session["TenantId"] == null)
            {
                TempData["Info"] = "Nastąpiło automatyczne wylogowanie. Proszę ponownie się zalogować";
                return RedirectToAction("Login", "Home");
            }
            else
            {
                int tenantId = (int)Session["TenantId"];
                HorseplayContext db = new HorseplayContext();
                var invs = db.Invitations.Where(i => i.TenantId == tenantId && i.InvitationId == id);
                if (invs.Any())
                {
                    return View(invs.FirstOrDefault());
                }
                else
                {
                    return new HttpNotFoundResult("Nie znaleziono strony");
                }
            }
        }

        public ActionResult DeleteInvitation(int id)
        {
            if (Session["TenantId"] == null)
            {
                TempData["Info"] = "Nastąpiło automatyczne wylogowanie. Proszę ponownie się zalogować";
                return RedirectToAction("Login", "Home");
            }
            else
            {
                int tenantId = (int)Session["TenantId"];
                HorseplayContext db = new HorseplayContext();
                var invs = db.Invitations.Where(i => i.InvitationId == id && i.TenantId == tenantId);
                if (invs.Any())
                {
                    return View(invs.FirstOrDefault());
                }else
                {
                    return new HttpNotFoundResult("Nie znaleziono strony");
                }
            }
        }

        [HttpPost]
        public ActionResult DeleteInvitation(Invitation Invitation)
        {
            HorseplayContext db = new HorseplayContext();
            int tenantId = (int)Session["TenantId"];
            var invs = db.Invitations.Where(i => i.TenantId == tenantId && i.InvitationId == Invitation.InvitationId);
            if (invs.Any())
            {
                Invitation orgInv = invs.FirstOrDefault();
                db.Invitations.Remove(orgInv);
                db.SaveChanges();
                return RedirectToAction("GetInvitations");
            }
            else
            {
                return new HttpNotFoundResult("Nie znaleziono strony");
            }  
        }

        public ActionResult SendInvitation()
        {
            if (Session["TenantId"] == null)
            {
                TempData["Info"] = "Nastąpiło automatyczne wylogowanie. Proszę ponownie się zalogować";
                return RedirectToAction("Login", "Home");
            }
            else
            {
                return View();
            }
        }
        [HttpPost]
        public ActionResult SendInvitation(Invitation Invitation)
        {
            if (Session["TenantId"] == null)
            {
                TempData["Info"] = "Nastąpiło automatyczne wylogowanie. Proszę ponownie się zalogować";
                return RedirectToAction("Login", "Home");
            }
            else
            {
                HorseplayContext db = new HorseplayContext();
                int tenantId = (int)Session["TenantId"];
                int userId = (int)Session["UserId"];
                var confirmedUsers = db.Users.Where(u => u.Email == Invitation.InvitedMail.Trim() && u.IsConfirmed == true);
                if (confirmedUsers.Any())
                {
                    ModelState.AddModelError(string.Empty, "Użytkownik " + Invitation.InvitedMail.Trim() + " zaakceptował zaproszenie w dniu " + confirmedUsers.FirstOrDefault().ConfirmationDate.ToString());
                    return View("SendInvitation", Invitation);
                }
                else
                {
                    DateTime currentDate = DateTime.Now.Date;
                    var users = db.Invitations.Where(u => u.InvitedMail == Invitation.InvitedMail.Trim() && u.ExpirationDate > currentDate && u.IsAccepted==false);
                    if (users.Any())
                    {
                        ModelState.AddModelError(string.Empty, "Do użytkownika " + Invitation.InvitedMail.Trim() + " zostało wysłane zaproszenie w dniu " + users.FirstOrDefault().DateAdded.ToString() + ". Tamto zaproszenie jest jeszcze ważne " + ((int)((users.FirstOrDefault().ExpirationDate - DateTime.Now).TotalDays)).ToString() + " dni. Dopiero po upływie tego terminu, o ile użytkownik wcześniej nie zaakceptuje zaproszenia, będzie można ponowić zaproszenie.");
                        return View("SendInvitation", Invitation);
                    }
                    else
                    {
                        //var currentUsers = db.Invitations.Where(u => u.InvitedMail == Invitation.InvitedMail.Trim() && u.IsAccepted == true);
                        //if (currentUsers.Any())
                        //{

                        //}
                        //We can invite this motherfucker
                        try
                        {
                            string token = Utilities.uniqueToken(db);
                            //try send mail first
                            Mail newMail = new Mail();
                            string iName = Utilities.UserName(userId);
                            string[] names = iName.Split(' ');
                            string textBody = "Użytkownik " + iName + " zaprasza Cię do dołączenia do jego firmy w Systo.pl! Aby przyjąć jego propozycję, skopiuj ten link do przeglądarki i utwórz swoje konto: http://systo.pl/Home/AcceptInvitation/" + token;
                            string htmlBody = "<h2 align=\"center\">Użytkownik " + iName + " zaprasza Cię do dołączenia do jego firmy w Systo.pl!</h2><br>";
                            htmlBody += "<p align=\"center\"><font size=\"4\">Aby przyjąć jego propozycję, kliknij poniższy link i utwórz swoje konto. System automatycznie połączy Twoje konto z kontem użytkownika " + names[0] + "</font></p>";
                            htmlBody += "<br><p align=\"center\"><a href=\"http://systo.pl/Home/AcceptInvitation/" + token + "\">systo.pl/Home/AcceptInvitation/" + token + "/</a></p>";
                            htmlBody += "<br><br><p align=\"center\"><font size=\"2\">Wiadomość zotała wysłana automatycznie, prosimy na nią nie odpowiadać</font></p>";
                            newMail.Send(Invitation.InvitedMail, iName + " zaprasza Cię do współpracy w Systo.pl", textBody , htmlBody);
                            Invitation nInv = new Invitation();
                            nInv.TenantId = tenantId;
                            nInv.InvitedMail = Invitation.InvitedMail;
                            nInv.InvitationToken = token;
                            nInv.DateAdded = DateTime.Now;
                            nInv.ExpirationDate = DateTime.Now.AddDays(14);
                            nInv.AddedBy = userId;
                            db.Invitations.Add(nInv);
                            db.SaveChanges();
                            TempData["Success"] = "Zaproszenie do użytkownika " + Invitation.InvitedMail + " zostało wysłane pomyślnie!";
                        }
                        catch(Exception ex)
                        {
                            TempData["Error"] = "Wystąpiły problem podczas próby wysłania zaproszenia do " + Invitation.InvitedMail + ".. " + ex.Message;
                        }

                        return RedirectToAction("GetInvitations");
                    }
                }
            }
        }
    }
}