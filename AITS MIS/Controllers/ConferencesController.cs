using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UCOnline.Models;
using Web.Framework.Controllers;

namespace UCOnline.Controllers
{
    public class ConferencesController : BaseController
    {
        public ConferencesController() : base(new ModelConferences())
        {
        }

        public override ActionResult Index()
        {
            ViewBag.Title = "Conferences - kNOw.Waste Management Platform";
            ViewBag.ThematicmenuHome = new ModelHome().ShowThematicMenuHome();
            ViewBag.Conferences = new ModelConferences().GetData();
            ViewBag.ConferencesPage = new ModelConferences().GetPage();
            return View();
        }

        public override ActionResult Detail()
        {
            ViewBag.Title = "Conferences - kNOw.Waste Management Platform";
            ViewBag.ThematicmenuHome = new ModelHome().ShowThematicMenuHome();
            ViewBag.Conferences = Model.GetData();
            return View();
        }
    }
}