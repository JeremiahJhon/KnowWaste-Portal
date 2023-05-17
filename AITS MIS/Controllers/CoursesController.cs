using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UCOnline.Models;
using Web.Framework.Controllers;

namespace UCOnline.Controllers
{
    public class CoursesController : BaseController
    {
        public CoursesController() : base(new ModelCourses())
        {
        }

        public override ActionResult Index()
        {
            ViewBag.Title = "E-Learning - kNOw.Waste Management Platform";
            ViewBag.ThematicmenuHome = new ModelHome().ShowThematicMenuHome();
            ViewBag.Body = ((ModelCourses)Model).ShowList();
            ViewBag.Curriculum = ((ModelCourses)Model).ShowCurriculum();
            return View();
        }

        public override ActionResult Detail()
        {
            var id = Request.QueryString["id"];
            ViewBag.ThematicmenuHome = new ModelHome().ShowThematicMenuHome();
            ViewBag.Detail = ((ModelCourses)Model).ShowDetail(id);

            return View();
        }
    }
}