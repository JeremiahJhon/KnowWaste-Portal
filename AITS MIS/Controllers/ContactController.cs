using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UCOnline.Models;
using Web.Framework.Controllers;

namespace UCOnline.Controllers
{
    public class ContactController : BaseController
    {
        public ContactController() : base(new ModelContact())
        {
        }

        public override ActionResult Index()
        {
            ViewBag.Title = "Contact Us - kNOw.Waste Management Platform";
            ViewBag.ThematicmenuHome = new ModelHome().ShowThematicMenuHome();
            ViewBag.Contact = ((ModelContact) Model).GetData();               
            return View();
        }

        public ActionResult About()
        {
            ViewBag.About = "";

            return View();
        }
    }
}