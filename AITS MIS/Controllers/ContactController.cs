using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
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

        [HttpPost]
        public ActionResult SendEmail(FormCollection formData)
        {
            SmtpClient ss = new SmtpClient("smtp.rrcap.ait.ac.th");
            ss.Port = 25;
            ss.UseDefaultCredentials = true;
            string asd = formData["email"];
            ss.Send(formData["email"], "warm@rrcap.ait.ac.th", formData["name"], formData["message"]);

            return RedirectToAction("Index", "Contact");
        }
    }
}