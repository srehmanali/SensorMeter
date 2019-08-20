using SensorMeter.Domain;
using SensorMeter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SensorMeter.Controllers
{
    public class NavbarController : Controller
    {
        // GET: Navbar
        public ActionResult Index()
        {
            var data = new Data();
            return PartialView("_Navbar", data.navbarItems().ToList());
        }
    }
}