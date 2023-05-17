using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UCOnline.Models;
using Web.Framework.Controllers;

namespace UCOnline.Controllers
{
    public class LawPolicyController : BaseController
    {
        public LawPolicyController() : base(new ModelLawPolicy())
        {
        }

        public override ActionResult Index()
        {
            ViewBag.Title = "Law/Policy - kNOw.Waste Management Platform";
            ViewBag.ThematicmenuHome = new ModelHome().ShowThematicMenuHome();
            ViewBag.Body = ((ModelLawPolicy) Model).ShowList();            
            return View();
        }

    }
}