﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

    }
}
