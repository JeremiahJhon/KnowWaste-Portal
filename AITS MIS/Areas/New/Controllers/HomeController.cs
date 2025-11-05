using System.Web.Mvc;
using KnowWaste.Models;

namespace Knowwaste.Areas.New.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Home model = new Home();
            model.GetData();
            return View(model);
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contacts()
        {
            return View();
        }
    }
}
