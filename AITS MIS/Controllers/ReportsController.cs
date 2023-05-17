using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UCOnline.Models;
using Web.Framework.Controllers;

namespace UCOnline.Controllers
{
    public class ReportsController : BaseController
    {
        public ReportsController() : base(new ModelReports())
        {
        }

        public override ActionResult Index()
        {
            ViewBag.Title = "Academic Report - kNOw.Waste Management Platform";
            ViewBag.ThematicmenuHome = new ModelHome().ShowThematicMenuHome();
            ViewBag.Body = ((ModelReports)Model).ShowList();
            return View();
        }
        
    }
}