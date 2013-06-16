using System.Web.Mvc;

namespace Cooperate_mvc.Controllers
{
    public class HomeController : Controller
    {

        //
        // GET: /Home/

        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Tasks");
            return View();
        }
    }
}
