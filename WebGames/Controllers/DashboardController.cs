using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebGames.Controllers
{
    [Authorize(Roles = "sysadmin,admin")]
    public class DashboardController: Controller
    {

    }
}