using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using WebGames.Libs;
using WebGames.Libs.Games;
using WebGames.Models;
using WebGames.Models.ViewModels;

namespace WebGames.Controllers
{
    [Authorize(Roles = "player")]
    public class UserController : Controller
    {
        public UserController()
        {
        }

        public UserController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }


        public ActionResult Index()
        {
            var model = new UserScoreInfo { GameScores = new List<UserScore>() };

            var userId = User.Identity.GetUserId();

            foreach (var game in GameManager.GameDict)
            {
                var Game_Score = game.Value.SM.GetUserScore(userId);

                model.GameScores.Add(Game_Score);
            }

            return View(model);
        }
    }
}