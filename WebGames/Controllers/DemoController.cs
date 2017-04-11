using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebGames.Controllers
{
    public class DemoController : Controller
    {
        public ActionResult Map()
        {
            return View();
        }

        public ActionResult Explainer()
        {
            return View();
        }

        public ActionResult Mastermind()
        {
            return View();
        }

        public ActionResult Outoftime()
        {
            return View();
        }

    }
}