using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UCOnline.Models;
using Web.Framework.Controllers;

namespace UCOnline.Controllers
{
    public class CountryPopulationController : BaseController
    {
        public CountryPopulationController() : base(new ModelCountryPopulation())
        {
        }

        public override ActionResult Index()
        {
            ViewBag.Title = "Country Population - kNOw.Waste Management Platform";
            ViewBag.ThematicmenuHome = new ModelHome().ShowThematicMenuHome();
            ViewBag.CountryPopulation = new ModelCountryPopulation().GetData();
            return View();
        }
    }
}