using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using WebGames.Libs;
using WebGames.Libs.Games;
using WebGames.Libs.Games.GameTypes;

namespace WebGames.Controllers
{
    public class GamesController: Controller
    {
        [Authorize(Roles = "player")]
        public ActionResult ActiveGame()
        {
            var PageToDisplay = "";

            var CurrentActiveGameKey = GameManager.GetActiveGameKey();

            if ((CurrentActiveGameKey ?? "") != "" && GameManager.GamePageDict.ContainsKey(CurrentActiveGameKey))
            {
                PageToDisplay = GameManager.GamePageDict[CurrentActiveGameKey];
            }
            else
            {
                PageToDisplay = "NoActiveGame";
            }

            return View(PageToDisplay); 
        }

        #region Game1
        [Authorize(Roles = "player")]
        public ActionResult SaveGame1Score(int score)
        {

            // Security - Check if Game is the currently active one - cannot set the score for a non active game
            var CurrentActiveGameKey = GameManager.GetActiveGameKey();
            if (CurrentActiveGameKey != Game1_Manager.GameKey)
            {
                return Json(new { success = false, message = "Game is not active" }, JsonRequestBehavior.AllowGet);
            }
            var UserId = User.Identity.GetUserId();
            Game1_Manager.SetUserScore(UserId, score, EnableOverride: false); // Cannot override the score - once is set the done

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Game6

        [Authorize(Roles = "player")]
        public ActionResult SaveGame6Score(int score)
        {
            // Security - Check if Game is the currently active one - cannot set the score for a non active game
            var CurrentActiveGameKey = GameManager.GetActiveGameKey();
            if (CurrentActiveGameKey != Game6_Manager.GameKey)
            {
                return Json(new { success = false, message = "Game is not active" }, JsonRequestBehavior.AllowGet);
            }

            var UserId = User.Identity.GetUserId();
            Game6_Manager.SetUserScore(UserId, score, EnableOverride: false); // Cannot override the score - once is set the done

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }


        #endregion


        [Authorize(Roles = "player")]
        public ActionResult SaveGameTime(int timeInSeconds)
        {
            var UserId = User.Identity.GetUserId();       
            ActivityManager.AddPlayedTimeForToday(UserId, timeInSeconds); // Cannot override the score - once is set the done
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

    }
}