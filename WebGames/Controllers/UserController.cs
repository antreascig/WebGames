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


        public async Task<ActionResult> Index()
        {
            var model = new UserScoreInfo { GameScores = new List<UserScore>() };

            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user == null)
            {
                return View(model);
            }
            var Game1_Score = GameManager.GameDict[GameKeys.GAME_1].SM.GetUserScore(user);
            //var Game2_Score = GameManager.GameDict[GameKeys.GAME_2].SM.GetUserScore(user);
            //var Game3_Score = GameManager.GameDict[GameKeys.GAME_3].SM.GetUserScore(user);
            //var Game4_1_Score = GameManager.GameDict[GameKeys.GAME_4_1"].SM.GetUserScore(user);
            //var Game4_2_Score = GameManager.GameDict["GameKeys.GAME_4_1"].SM.GetUserScore(user);
            //var Game4_3_Score = GameManager.GameDict["GameKeys.GAME_4_3"].SM.GetUserScore(user);
            //var Game5_Score = GameManager.GameDict["GameKeys.GAME_5"].SM.GetUserScore(user);
            //var Game6_Score = GameManager.GameDict["GameKeys.GAME_6"].SM.GetUserScore(user);

            //var Game4_Score = new UserScore()
            //{
            //    GameId = -1,
            //    GameName = "",
            //    Name = "Escape Room",
            //    Score = Game4_1_Score.Score + Game4_2_Score.Score + Game4_3_Score.Score,
            //    UserId = user.Id
            //};

            model.GameScores.Add(Game1_Score);
            //model.GameScores.Add(Game2_Score);
            //model.GameScores.Add(Game3_Score);
            //model.GameScores.Add(Game4_Score);
            //model.GameScores.Add(Game5_Score);
            //model.GameScores.Add(Game6_Score);

            return View(model);
        }
    }
}