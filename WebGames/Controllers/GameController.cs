using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using WebGames.Libs;
using WebGames.Libs.Games;

namespace WebGames.Controllers
{
    public class GamesController: Controller
    {
        [Authorize(Roles = "player")]
        public ActionResult SaveScore(int score)
        {
            // Security - Check if Game is the currently active one - cannot set the score for a non active game
            var GameId = GameManager.GetActiveGameId();
            var UserId = User.Identity.GetUserId();
            ScoreManager.SetUserScore(GameId, UserId, score, EnableOverride: false); // Cannot override the score - once is set the done

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "player")]
        public ActionResult SaveGameTime(int timeInSeconds)
        {
            var UserId = User.Identity.GetUserId();       
            ActivityManager.AddPlayedTimeForToday(UserId, timeInSeconds); // Cannot override the score - once is set the done
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

    }
}