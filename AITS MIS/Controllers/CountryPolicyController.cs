using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UCOnline.Models;
using Web.Framework.Controllers;

namespace UCOnline.Controllers
{
    public class CountryPolicyController : BaseController
    {
        public CountryPolicyController() : base(new ModelCountryPolicy())
        {
        }

        public override ActionResult Index()
        {
            ViewBag.Title = "Country Policies - kNOw.Waste Management Platform";
            ViewBag.ThematicmenuHome = new ModelHome().ShowThematicMenuHome();
            ViewBag.CountryPolicy = new ModelCountryPolicy().GetData();
            return View();
        }
    }
}