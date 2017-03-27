using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;

namespace WebGames.Controllers
{
    //[Authorize]
    public class ValuesController : Controller
    {
        // GET api/values/admin
        [Authorize(Roles = "sysadmin")]
        public ActionResult Admin()
        {
            var userName = User.Identity.Name;
            return Content($"Hello Admin, {userName}.");
        }

        // GET api/values/player
        [Authorize(Roles = "player")]
        public ActionResult Player()
        {
            var userName = User.Identity.Name;
            return Content($"Hello Player, {userName}.");

        }
    }

}
