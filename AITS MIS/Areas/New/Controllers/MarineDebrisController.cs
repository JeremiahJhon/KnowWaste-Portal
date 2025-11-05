using KnowWaste.Models;
using System.Web.Mvc;

namespace Knowwaste.Areas.New.Controllers
{
    public class MarineDebrisController : Controller
    {
        // GET: New/MarineDebris
        public ActionResult Index()
        {
            MarineDebris model = new MarineDebris();
            model.GetData();
            return View(model);
        }
    }
}