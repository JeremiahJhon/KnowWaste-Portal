using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UCOnline.Models;
using Web.Framework.Controllers;

namespace UCOnline.Controllers
{
    public class PagesController : BaseController
    {
        public PagesController() : base(new ModelPages())
        {
        }

        public override ActionResult Index()
        {
            ViewBag.Title = "Index";
            
            return View();
        }

        public ActionResult Empty()
        {
            ViewBag.Title = "Empty";

            return View();
        }

        public ActionResult Repair()
        {
            ViewBag.Title = "Repair";

            return View();
        }

        public ActionResult News()
        {
            ViewBag.Title = "News";

            return View();
        }
    }
}