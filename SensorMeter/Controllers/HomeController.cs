using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SensorMeter.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Setup()
        {
            return View("Setup");
        }
        public ActionResult Process()
        {
            return View("Process");
        }
        public ActionResult Report()
        {
            return View("Report");
        }
        public ActionResult Settings()
        {
            return View("Settings");
        }
        public ActionResult Login()
        {
            return View("Login");
        }
        public ActionResult Blank()
        {
            return View("Blank");
        }
        public ActionResult Error()
        {
            return View("Error");
        }






        public ActionResult FlotCharts()
        {
            return View("FlotCharts");
        }

        public ActionResult MorrisCharts()
        {
            return View("MorrisCharts");
        }

        public ActionResult Tables()
        {
            return View("Tables");
        }

        public ActionResult Forms()
        {
            return View("Forms");
        }

        public ActionResult Panels()
        {
            return View("Panels");
        }

        public ActionResult Buttons()
        {
            return View("Buttons");
        }

        public ActionResult Notifications()
        {
            return View("Notifications");
        }

        public ActionResult Typography()
        {
            return View("Typography");
        }

        public ActionResult Icons()
        {
            return View("Icons");
        }

        public ActionResult Grid()
        {
            return View("Grid");
        }

    }
}