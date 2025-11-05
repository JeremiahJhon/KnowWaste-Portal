using KnowWaste.Models;
using System.Web.Mvc;

namespace Knowwaste.Areas.New.Controllers
{
    public class ExpertsController : Controller
    {
        // GET: New/_3RproMar
        public ActionResult Index()
        {
            Experts model = new Experts();
            model.GetData();
            return View(model);
        }
    }
}