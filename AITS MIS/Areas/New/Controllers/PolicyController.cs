using KnowWaste.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Knowwaste.Areas.New.Controllers
{
    public class PolicyController : Controller
    {
        // GET: New/Policy
        public ActionResult Index()
        {
            Policy model = new Policy(0, new List<string>());
            return View(model);
        }

        // POST: New/Policy/RefreshData
        [HttpPost]
        public ActionResult RefreshData(int countryID, List<string> data)
        {
            if (data == null) { data = new List<string>(); }
            Policy model = new Policy(countryID, data);
            return PartialView("_Data", model);
        }
    }
}