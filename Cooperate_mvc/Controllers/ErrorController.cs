using System.Web.Mvc;

namespace Cooperate_mvc.Controllers
{
    public class ErrorController : Controller
    {
        //
        // GET: /Error/

        public ActionResult InsufficientRights()
        {
            return View();
        }

        public ActionResult Error()
        {
            return View();
        }

    }
}
