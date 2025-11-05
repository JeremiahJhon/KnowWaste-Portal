using KnowWaste.Models;
using System.Web.Mvc;

namespace Knowwaste.Areas.New.Controllers
{
    public class EventsController : Controller
    {
        // GET: New/Events
        public ActionResult Index()
        {
            return View();
        }

        // GET: New/Events/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
    }
}
