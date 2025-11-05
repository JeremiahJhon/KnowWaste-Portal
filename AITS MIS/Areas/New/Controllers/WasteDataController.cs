using KnowWaste.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Knowwaste.Areas.New.Controllers
{
    public class WasteDataController : Controller
    {
        // GET: New/WasteData
        public ActionResult Index()
        {
            return View();
        }
    }
}