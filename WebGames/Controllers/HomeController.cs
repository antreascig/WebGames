using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LocalAccountsApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Register()
        {
            ViewBag.Title = "Registration Page";

            return View("Register");
        }

        public ActionResult Login()
        {
            ViewBag.Title = "Login Page";

            return View("Login");
        }

        public ActionResult Index()
        {
            if ( !User.Identity.IsAuthenticated)
            {
                return Login();
            }

            ViewBag.Title = "Home Page";

            return View("Index");
        }
    }
}
