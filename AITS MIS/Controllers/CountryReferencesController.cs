using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UCOnline.Models;
using Web.Framework.Controllers;

namespace UCOnline.Controllers
{
    public class CountryReferencesController : BaseController
    {
        public CountryReferencesController() : base(new ModelCountryReferences())
        {
        }

        public override ActionResult Index()
        {
            ViewBag.Title = "Country References - kNOw.Waste Management Platform";
            ViewBag.ThematicmenuHome = new ModelHome().ShowThematicMenuHome();
            ViewBag.CountryReferences = new ModelCountryReferences().GetData();
            return View();
        }
    }
}