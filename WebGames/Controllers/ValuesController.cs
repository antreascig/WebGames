using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LocalAccountsApp.Controllers
{
    //[Authorize]
    [RoutePrefix("api/values")]
    public class ValuesController : ApiController
    {
        // GET api/values/admin
        [Authorize(Roles = "sysadmin")]
        [Route("Admin")]
        [HttpGet]
        public string Admin()
        {
            var userName = this.RequestContext.Principal.Identity.Name;
            return String.Format("Hello Admin, {0}.", userName);
        }

        // GET api/values/player
        [Authorize(Roles = "player")]
        [Route("Player")]
        [HttpGet]
        public string Player()
        {
            var userName = this.RequestContext.Principal.Identity.Name;
            return String.Format("Hello Player, {0}.", userName);
        }
    }

}
