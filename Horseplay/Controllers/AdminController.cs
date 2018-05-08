using Horseplay.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Horseplay.Models;

namespace Horseplay.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        //[Authorize(Roles ="Admin")]
        public ActionResult Users()
        {
            HorseplayContext db = new HorseplayContext();
            var users = db.Users.ToList();
            return View(users);
        }
    }
}