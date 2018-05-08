using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Horseplay.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Static
        public ActionResult UnderConstruction()
        {
            return View("UnderConstruction");
        }
    }
}