using Horseplay.DAL;
using Horseplay.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Horseplay.Controllers
{
    public class CompanyGroupController : Controller
    {
        // GET: CompanyGroup
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddCompanyGroup(CompanyGroup companyGroup)
        {
            if (!ModelState.IsValid)
            {
                return View("Add",companyGroup);
            }else
            {
                HorseplayContext db = new HorseplayContext();
                if (db.CompanyGroups.Any(x => x.Name == companyGroup.Name))
                {
                    ModelState.AddModelError(string.Empty, "Grupa o takiej nazwie już istnieje!");
                    return View("Add",companyGroup);
                }else
                {
                    db.CompanyGroups.Add(companyGroup);
                    db.SaveChanges();
                    return View("companyGroupCreated",companyGroup);
                }
            }
            
        }
    }
}