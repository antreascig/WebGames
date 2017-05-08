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
using WebGames.Libs.Games.Games;
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
            var userId = User.Identity.GetUserId();

            var model = new UserScoreInfo { UserId = userId, GameScores = new List<UserScore>(), GroupScore = null };

            var EscapeKeys = new string[] { GameKeys.Escape_1, GameKeys.Escape_2, GameKeys.Escape_3 };
            double EscapeScore = 0;

            foreach (var game in GameManager.GameDict)
            {
                var Game_Score = game.Value.SM.GetUserScore(userId);

                if (EscapeKeys.Contains(game.Key))
                {
                    EscapeScore += Game_Score.Score;
                    continue;
                }

                model.GameScores.Add(Game_Score);
            }

            model.GameScores.Add(new UserScore() { GameId = -1, GameName = "ΚΛΟΥΒΙΑ ΚΛΟΥΒΙΑ", Score = EscapeScore });

            model.GroupScore = Group_Manager.GetUserGroupScore(userId);

            return View(model);
        }
    }
}