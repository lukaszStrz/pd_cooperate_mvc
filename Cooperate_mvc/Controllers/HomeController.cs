using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
                return Wall();
            return View();
        }

        //
        // GET: /Home/Wall/

        [Authorize]
        public ActionResult Wall()
        {
            return View();
        }

    }
}
