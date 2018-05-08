using Horseplay.DAL;
using Horseplay.Models;
using Horseplay.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Horseplay.Controllers
{
    public class HRController : Controller
    {
        // GET: HR
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

        public ActionResult GetEmployeeGroups()
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
                var employeeGroups = db.EmployeeGroups.Where(eg => eg.TenantId == tenantId);
                return View(employeeGroups);
            }
        }

        public ActionResult CreateEmployeeGroup()
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

        public ActionResult CreateEmployeeGroup(EmployeeGroup eg)
        {
            if (!ModelState.IsValid)
            {
                return View("CreateEmployeeGroup", eg);
            }
            else
            {
                HorseplayContext db = new HorseplayContext();
                int tenantId = (int)Session["TenantId"];
                if (db.EmployeeGroups.Any(x => x.Name == eg.Name && x.TenantId==tenantId))
                {
                    ModelState.AddModelError(string.Empty, "Grupa o takiej nazwie już istnieje. Podaj unikatową nazwę grupy");
                    return View("CreateEmployeeGroup", eg);
                }
                else
                {
                    eg.addedBy = (int)Session["UserId"];
                    eg.dateAdded = DateTime.Now;
                    eg.TenantId = tenantId;
                    db.EmployeeGroups.Add(eg);
                    db.SaveChanges();
                    return RedirectToAction("GetEmployeeGroups");
                }
            }

        }
        
        public ActionResult EmployeeGroupDetails(int id)
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
                var egs = db.EmployeeGroups.Where(e => e.EmployeeGroupId == id && e.TenantId == tenantId);
                if (egs.Any())
                {
                    return View(egs.FirstOrDefault());
                }
                else
                {
                    return new HttpNotFoundResult("Nie znaleziono strony");
                }
            }
        }

        public ActionResult EditEmployeeGroup(int id)
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
                var egs = db.EmployeeGroups.Where(e => e.EmployeeGroupId == id && e.TenantId == tenantId);
                if (egs.Any())
                {
                    return View(egs.FirstOrDefault());
                }
                else
                {
                    return new HttpNotFoundResult("Nie znaleziono strony");
                }
            }
        }

        [HttpPost]
        public ActionResult EditEmployeeGroup(EmployeeGroup eg)
        {
            if (!ModelState.IsValid)
            {
                return View("EditEmployeeGroup", eg);
            }
            else
            {
                HorseplayContext db = new HorseplayContext();
                int tenantId = (int)Session["TenantId"];
                int userId = (int)Session["UserId"];
                if (db.EmployeeGroups.Any(x => x.Name == eg.Name && x.TenantId==tenantId && x.EmployeeGroupId!=eg.EmployeeGroupId))
                {
                    ModelState.AddModelError(string.Empty, "Grupa o takiej nazwie już istnieje. Podaj unikatową nazwę grupy");
                    return View("EditEmployeeGroup", eg);
                }
                else
                {
                    var oldEg = db.EmployeeGroups.FirstOrDefault(e => e.EmployeeGroupId == eg.EmployeeGroupId && e.TenantId == tenantId);
                    if (oldEg == null)
                    {
                        return new HttpNotFoundResult("Wystąpił nieoczekiwany błąd");
                    }else
                    {
                        oldEg.Name = eg.Name;
                        oldEg.dateModified = DateTime.Now;
                        oldEg.modifiedBy = userId;
                        db.Entry(oldEg).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("GetEmployeeGroups");
                    }  
                }
            }
        }

        public ActionResult DeleteEmployeeGroup(int id)
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
                var egs = db.EmployeeGroups.Where(e => e.EmployeeGroupId == id && e.TenantId == tenantId);
                if (egs.Any())
                {
                    return View(egs.FirstOrDefault());
                }
                else
                {
                    return new HttpNotFoundResult("Nie znaleziono strony");
                }
            }
        }

        [HttpPost]
        public ActionResult DeleteEmployeeGroup(EmployeeGroup eg)
        {
            HorseplayContext db = new HorseplayContext();
            int tenantId = (int)Session["TenantId"];
            var oldEg = db.EmployeeGroups.FirstOrDefault(e => e.EmployeeGroupId == eg.EmployeeGroupId && e.TenantId == tenantId);
            if (oldEg == null)
            {
                return new HttpNotFoundResult("Wystąpił nieoczekiwany błąd");
            }
            else
            {
                db.EmployeeGroups.Remove(oldEg);
                db.SaveChanges();
                return RedirectToAction("GetEmployeeGroups");
            }
        }

        public ActionResult GetEmployees()
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
                var employees = db.Employees.Include("Groups").Where(e => e.TenantId == tenantId);

                return View(employees);
            }
        }

        public ActionResult CreateEmployee()
        {
            if (Session["TenantId"] == null)
            {
                TempData["Info"] = "Nastąpiło automatyczne wylogowanie. Proszę ponownie się zalogować";
                return RedirectToAction("Login", "Home");
            }
            else
            {
                HorseplayContext db = new HorseplayContext();
                HRCreateEmployeeViewModel vm = new HRCreateEmployeeViewModel();
                int tenantId = (int)Session["TenantId"];
                vm.employeeGroups = db.EmployeeGroups.Where(eg => eg.TenantId == tenantId);
                return View(vm);
            }
        }

        [HttpPost]
        public ActionResult CreateEmployee(HRCreateEmployeeViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View("CreateEmployee",vm);
            }
            else
            {
                HorseplayContext db = new HorseplayContext();
                vm.employee.TenantId = (int)Session["TenantId"];
                vm.employee.addedBy = (int)Session["UserId"];
                vm.employee.dateAdded = DateTime.Now;
                List<EmployeeGroup> groups = new List<EmployeeGroup>();
                if (vm.selectedGroups != null)
                {
                    foreach (int i in vm.selectedGroups)
                    {
                        var result = (from eg in db.EmployeeGroups
                                      where eg.EmployeeGroupId == i
                                      select eg);
                        foreach (var emp in result)
                        {
                            groups.Add(emp);
                        }
                    }
                }
                
                if (groups.Any())
                {
                    vm.employee.Groups = groups;
                }
                db.Employees.Add(vm.employee);
                db.SaveChanges();
                return RedirectToAction("GetEmployees");
                
            }
        }

        public ActionResult EmployeeDetails(int id)
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
                var es = db.Employees.Where(e => e.EmployeeId == id && e.TenantId == tenantId);
                if (es.Any())
                {
                    return View(es.FirstOrDefault());
                }
                else
                {
                    return new HttpNotFoundResult("Nie znaleziono strony");
                }
            }
        }

        public ActionResult EditEmployee(int id)
        {
            if (Session["TenantId"] == null)
            {
                TempData["Info"] = "Nastąpiło automatyczne wylogowanie. Proszę ponownie się zalogować";
                return RedirectToAction("Login", "Home");
            }
            else
            {
                HorseplayContext db = new HorseplayContext();
                HRCreateEmployeeViewModel vm = new HRCreateEmployeeViewModel();
                int tenantId = (int)Session["TenantId"];
                var es = db.Employees.Include("Groups").Where(e => e.EmployeeId == id && e.TenantId == tenantId);

                if (es.Any())
                {
                    vm.employee = es.FirstOrDefault();
                    vm.employeeGroups = db.EmployeeGroups.Where(e => e.TenantId == tenantId);
                    foreach (var item in vm.employeeGroups)
                    {
                        if (vm.employee.BelongsToGroup(item.EmployeeGroupId))
                        {
                            if (vm.selectedGroups == null)
                            {
                                vm.selectedGroups = new List<int>();
                            }
                            vm.selectedGroups.Add(item.EmployeeGroupId);
                        }
                    }
                    return View(vm);
                }
                else
                {
                    return new HttpNotFoundResult("Nie znaleziono strony");
                }
            }
        }

        [HttpPost]
        public ActionResult EditEmployee(HRCreateEmployeeViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View("EditEmployee",vm);
            }else
            {
                int userId = (int)Session["UserId"];
                HorseplayContext db = new HorseplayContext();
                Employee oldEmp = db.Employees.Where(e => e.EmployeeId == vm.employee.EmployeeId).First();
                oldEmp.Name = vm.employee.Name;
                oldEmp.Surname = vm.employee.Surname;
                oldEmp.Country = vm.employee.Country;
                oldEmp.ContractType = vm.employee.ContractType;
                oldEmp.PersonalIdentityNumber = vm.employee.PersonalIdentityNumber;
                oldEmp.IdNumber = vm.employee.IdNumber;
                oldEmp.ExpirationDate = vm.employee.ExpirationDate;
                oldEmp.modifiedBy = userId;
                oldEmp.dateModified = DateTime.Now;
                oldEmp.Mail = vm.employee.Mail;
                oldEmp.Mobile = vm.employee.Mobile;
                oldEmp.Phone = vm.employee.Phone;
                oldEmp.Address = vm.employee.Address;
                oldEmp.BirthDay = vm.employee.BirthDay;
                oldEmp.EmployedOn = vm.employee.EmployedOn;
                oldEmp.Groups.Clear();

                if (vm.selectedGroups != null)
                {
                    foreach (int i in vm.selectedGroups)
                    {
                        var result = (from eg in db.EmployeeGroups
                                      where eg.EmployeeGroupId == i
                                      select eg);
                        foreach (var emp in result)
                        {
                            oldEmp.Groups.Add(emp);
                        }
                    }
                }
                
                db.Entry(oldEmp).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("GetEmployees");
            }
        }

        public ActionResult DeleteEmployee(int id)
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
                var es = db.Employees.Where(e => e.EmployeeId == id && e.TenantId == tenantId);
                if (es.Any())
                {
                    return View(es.FirstOrDefault());
                }
                else
                {
                    return new HttpNotFoundResult("Nie znaleziono strony");
                }
            }
        }

        [HttpPost]
        public ActionResult DeleteEmployee(Employee ee)
        {
            HorseplayContext db = new HorseplayContext();
            int tenantId = (int)Session["TenantId"];
            var oldEg = db.Employees.FirstOrDefault(e => e.EmployeeId == ee.EmployeeId && e.TenantId == tenantId);
            if (oldEg == null)
            {
                return new HttpNotFoundResult("Wystąpił nieoczekiwany błąd");
            }
            else
            {
                db.Employees.Remove(oldEg);
                db.SaveChanges();
                return RedirectToAction("GetEmployees");
            }
        }
    }
}