using Horseplay.DAL;
using Horseplay.Models;
using Horseplay.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace Horseplay.Controllers
{
    public class CRMController : Controller
    {
        // GET: CRM
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

        public ActionResult CreateCompany()
        {
            if (Session["TenantId"] == null)
            {
                TempData["Info"] = "Nastąpiło automatyczne wylogowanie. Proszę ponownie się zalogować";
                return RedirectToAction("Login", "Home");
            }
            else
            {
                HorseplayContext db = new HorseplayContext();
                CompanyViewModel vm = new CompanyViewModel();
                int tenantId = (int)Session["TenantId"];
                return View(vm);
            }
        }

        [HttpPost]
        public ActionResult CreateCompany(CompanyViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View("CreateCompany", vm);
            }else
            {
                HorseplayContext db = new HorseplayContext();
                int tenantId = (int)Session["TenantId"];
                int userId = (int)Session["UserId"];
                vm.Company.TenantId = tenantId;
                vm.Company.AddedBy = userId;
                vm.Company.DateAdded = DateTime.Now;
                if (vm.Company.Address != null)
                {
                    Address Address = new Address();
                    Address.AddedBy = userId;
                    Address.TenantId = tenantId;
                    Address.DateAdded = DateTime.Now;
                    Address.Name = vm.Company.Address.Name;
                    Address.Street = vm.Company.Address.Street;
                    Address.ZipCode = vm.Company.Address.ZipCode;
                    Address.City = vm.Company.Address.City;
                    Address.Country = vm.Company.Address.Country;
                    vm.Company.Address = Address;
                }
                if (vm.Company.ContactDetail != null)
                {
                    ContactDetail CD = new ContactDetail()
                    {
                        AddedBy = userId,
                        DateAdded = DateTime.Now,
                        TenantId = tenantId,
                        Phone = vm.Company.ContactDetail.Phone,
                        Mobile = vm.Company.ContactDetail.Mobile,
                        PrivatePhone = vm.Company.ContactDetail.PrivatePhone,
                        Fax = vm.Company.ContactDetail.Fax,
                        Mail = vm.Company.ContactDetail.Mail,
                        PrivateMail = vm.Company.ContactDetail.PrivateMail
                    };
                    vm.Company.ContactDetail = CD;
                }
                Company Company = new Company()
                {
                    AddedBy = userId,
                    DateAdded = DateTime.Now,
                    TenantId = tenantId,
                    Type = vm.Company.Type,
                    BusinessRegisterNumber = vm.Company.BusinessRegisterNumber,
                    CourtRegisterNumber = vm.Company.CourtRegisterNumber,
                    TaxRegisterNumber = vm.Company.TaxRegisterNumber,
                    PaymentTerm = vm.Company.PaymentTerm,
                    Address = vm.Company.Address,
                    ContactDetail = vm.Company.ContactDetail
                    
                };
                db.Companies.Add(vm.Company);
                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var error in e.EntityValidationErrors)
                    {
                        foreach (var propertyError in error.ValidationErrors)
                        {
                            Console.WriteLine($"{propertyError.PropertyName} had the following issue: {propertyError.ErrorMessage}");
                        }
                    }
                }
                return RedirectToAction("GetCompanies");
            }   
        }

        public ActionResult GetCompanies()
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
                var comps = db.Companies
                    .Include(c => c.Address)
                    .Include(c => c.ContactDetail)
                    .Where(c => c.TenantId == tenantId);
                return View(comps);
            }
        }

        public ActionResult CompanyDetails(int id)
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
                var comp = db.Companies
                    .Include(c => c.Address)
                    .Include(c => c.ContactDetail)
                    .Where(c => c.TenantId == tenantId && c.CompanyId == id);
                if (comp.Any())
                {
                    return View(comp.FirstOrDefault());
                }
                else
                {
                    return new HttpNotFoundResult("Nie znaleziono strony");
                }
            }   
        }

        public ActionResult DeleteCompany(int id)
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
                var comps = db.Companies
                    .Include(c => c.Address)
                    .Include(c => c.ContactDetail)
                    .Where(c => c.TenantId == tenantId && c.CompanyId == id);
                if (comps.Any())
                {
                    return View(comps.FirstOrDefault());
                }
                else
                {
                    return new HttpNotFoundResult("Nie znaleziono strony");
                }
            }
        }

        [HttpPost]
        public ActionResult DeleteCompany(Company comp)
        {
            HorseplayContext db = new HorseplayContext();
            int tenantId = (int)Session["TenantId"];
            var comps = db.Companies
                .Include(c => c.Address)
                .Include(c => c.ContactDetail)
                .Where(c => c.TenantId == tenantId && c.CompanyId == comp.CompanyId);
            if (comps.Any())
            {
                Company oldComp = comps.FirstOrDefault();
                Address oldAddress = db.Addresses.Where(a => a.AddressId == oldComp.Address.AddressId).FirstOrDefault();
                ContactDetail oldCD = db.ContactDetails.Where(a => a.ContactDetailId == oldComp.ContactDetail.ContactDetailId).FirstOrDefault();
                db.Addresses.Remove(oldAddress);
                db.SaveChanges();
                db.ContactDetails.Remove(oldCD);
                db.SaveChanges();
                //db.Companies.Remove(oldComp);
                //db.SaveChanges();
                return View("GetCompanies");
            }
            else
            {
                return new HttpNotFoundResult("Nieoczekiwany błąd");
            }
        }

        public ActionResult EditCompany(int id)
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
                var comps = db.Companies
                    .Include(c => c.Address)
                    .Include(c => c.ContactDetail)
                    .Where(c => c.CompanyId == id && c.TenantId == tenantId);

                if (comps.Any())
                {
                    CompanyViewModel vm = new CompanyViewModel();
                    vm.Company = comps.FirstOrDefault();
                    return View(vm);
                }
                else
                {
                    return new HttpNotFoundResult("Nieoczekiwany błąd");
                }
            }
        }

        [HttpPost]

        public ActionResult EditCompany(CompanyViewModel vm)
        {
            if (Session["TenantId"] == null)
            {
                TempData["Info"] = "Nastąpiło automatyczne wylogowanie. Proszę ponownie się zalogować";
                return RedirectToAction("Login", "Home");
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    return View("EditCompany", vm);
                }
                else
                {
                    HorseplayContext db = new HorseplayContext();
                    int tenantId = (int)Session["TenantId"];
                    var comps = db.Companies
                        .Include(c => c.Address)
                        .Include(c => c.ContactDetail)
                        .Where(c => c.CompanyId == vm.Company.CompanyId && c.TenantId == tenantId);
                    if (comps.Any())
                    {
                        int userId = (int)Session["UserId"];
                        Company oldComp = comps.FirstOrDefault();
                        oldComp.Type = vm.Company.Type;
                        oldComp.PaymentTerm = vm.Company.PaymentTerm;
                        oldComp.TaxRegisterNumber = vm.Company.TaxRegisterNumber;
                        oldComp.CourtRegisterNumber = vm.Company.CourtRegisterNumber;
                        oldComp.BusinessRegisterNumber = vm.Company.BusinessRegisterNumber;
                        oldComp.DateModified = DateTime.Now;
                        oldComp.ModifiedBy = userId;
                        oldComp.Address.Name = vm.Company.Address.Name;
                        oldComp.Address.Street = vm.Company.Address.Street;
                        oldComp.Address.ZipCode = vm.Company.Address.ZipCode;
                        oldComp.Address.City = vm.Company.Address.City;
                        oldComp.Address.Country = vm.Company.Address.Country;
                        oldComp.Address.ModifiedBy = userId;
                        oldComp.Address.DateModified = DateTime.Now;
                        oldComp.ContactDetail.DateModified = DateTime.Now;
                        oldComp.ContactDetail.ModifiedBy = userId;
                        oldComp.ContactDetail.Phone = vm.Company.ContactDetail.Phone;
                        oldComp.ContactDetail.Mobile = vm.Company.ContactDetail.Mobile;
                        oldComp.ContactDetail.Mail = vm.Company.ContactDetail.Mail;
                        oldComp.ContactDetail.Fax = vm.Company.ContactDetail.Fax;
                        db.Entry(oldComp).State = EntityState.Modified;
                        try
                        {
                            db.SaveChanges();
                        }
                        catch (DbEntityValidationException e)
                        {
                            foreach (var error in e.EntityValidationErrors)
                            {
                                foreach (var propertyError in error.ValidationErrors)
                                {
                                    Console.WriteLine($"{propertyError.PropertyName} had the following issue: {propertyError.ErrorMessage}");
                                }
                            }
                        }
                        return RedirectToAction("GetCompanies");
                    }
                    else
                    {
                        return new HttpNotFoundResult("Nieoczekiwany błąd");
                    }
                }
            }
        }
    }
}