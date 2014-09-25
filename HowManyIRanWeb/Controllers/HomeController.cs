using System.Web.Mvc;

namespace HowManyIRanWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}