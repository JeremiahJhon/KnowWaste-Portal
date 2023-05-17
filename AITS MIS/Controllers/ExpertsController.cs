using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UCOnline.Models;
using Web.Framework.Controllers;

namespace UCOnline.Controllers
{
    public class ExpertsController : BaseController
    {
        public ExpertsController() : base(new ModelExperts())
        {
        }

        public override ActionResult Index()
        {           
            ViewBag.Title = "Expert Rosters - kNOw.Waste Management Platform";
            ViewBag.ThematicmenuHome = new ModelHome().ShowThematicMenuHome();            
            ViewBag.Rosters = new ModelExperts().GetData();
            return View();
        }
    }
}