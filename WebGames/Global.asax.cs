using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace WebGames
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);


            //var SM = new Libs.Games.ScoreManager<Models.Game1_UserScore>();
            //var UserId = "ea46d952-c92f-4ad4-9d22-0971a9128e95";
            //SM.SetUserScore(UserId, 5, false);

            //SM.SetUserScore(UserId, 10, true);
        }
    }
}
