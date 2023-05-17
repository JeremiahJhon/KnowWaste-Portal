using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UCOnline.Models;
using Web.Framework.Controllers;

namespace UCOnline.Controllers
{
    public class CountryWasteStreamsController : BaseController
    {
        public CountryWasteStreamsController() : base(new ModelCountryWasteStreams())
        {
        }

        public override ActionResult Index()
        {
            ViewBag.Title = "Waste Data Streams - kNOw.Waste Management Platform";
            ViewBag.ThematicmenuHome = new ModelHome().ShowThematicMenuHome();
            ViewBag.CountryWasteStreams = new ModelCountryWasteStreams().GetData();
            ViewBag.Country = new ModelCountry().GetData();
            ViewBag.CountryPolicy = new ModelCountryPolicy().GetData();
            ViewBag.CountryPopulation = new ModelCountryPopulation().GetData();
            ViewBag.CountryReferences = new ModelCountryReferences().GetData();              
        
            return View();
        }
        
    }
}