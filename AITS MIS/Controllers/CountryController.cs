using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UCOnline.Models;
using Web.Framework.Controllers;

namespace UCOnline.Controllers
{
    public class CountryController : BaseController
    {
        public CountryController() : base(new ModelCountry())
        {
        }

        public override ActionResult Index()
        {            
            ViewBag.Title = "Country - kNOw.Waste Management Platform";
            ViewBag.ThematicmenuHome = new ModelHome().ShowThematicMenuHome();
            ViewBag.Country = new ModelCountry().GetData();
            
            return View();
        }
    }
}