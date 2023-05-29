using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UCOnline.Models;
using Web.Framework.Controllers;

namespace UCOnline.Controllers
{
    public class AboutController : BaseController
    {
        public AboutController() : base(new ModelAbout())
        {
        }

        public override ActionResult Index()
        {
            ViewBag.Title = "About Us";
            
            return View();
        }
    }
}