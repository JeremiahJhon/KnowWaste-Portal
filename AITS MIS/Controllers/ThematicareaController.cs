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
    public class ThematicareaController : BaseController
    {
        public ThematicareaController() : base(new ModelThematicarea())
        {
        }

        public override ActionResult Index()
        {
            ViewBag.Title = "8 Thematic Areas";

            var geothemeid = Request.QueryString["gid"];
            var doc = Request.QueryString["doc"];

            ((ModelThematicarea)Model).geothemeid = geothemeid;

            ViewBag.ThematicmenuHome = new ModelHome().ShowThematicMenuHome();
            ViewBag.Thematicmenu = ((ModelThematicarea)Model).ShowMenu();
            ViewBag.Thematicheader = ((ModelThematicarea)Model).ShowHeader();
            ViewBag.ThematicDescription = ((ModelThematicarea)Model).ShowDescription();

            if (doc != null)
            {
                ViewBag.Document = ((ModelThematicarea)Model).ShowDocument(doc);
                //ViewBag.Document = new ModelReports().ShowThumbnail(6);
            }
            else
            {
                ViewBag.LawPolicy = ((ModelThematicarea)Model).ShowListLawPolicy(geothemeid, 9);
                ViewBag.CaseStudy = ((ModelThematicarea)Model).ShowListCaseStudy(geothemeid, 9);
                ViewBag.Reports = ((ModelThematicarea)Model).ShowListReports(geothemeid, 9);
            }

            return View();
        }

    }
}