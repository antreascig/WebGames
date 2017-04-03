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

            // FOR TESTING
            PageToDisplay = "mastermind/Cosmoplay";
            //return new FilePathResult(PageToDisplay, "text/html");
            return View(PageToDisplay);
        }

        #region Game1

        [Authorize(Roles = "player")]
        public ActionResult Save_Game1_Score(int score)
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

        #region Game2

        [Authorize(Roles = "player")]
        public ActionResult Save_Game2_Score(string stage)
        {

            // Security - Check if Game is the currently active one - cannot set the score for a non active game
            var CurrentActiveGameKey = GameManager.GetActiveGameKey();
            if (CurrentActiveGameKey != Game1_Manager.GameKey)
            {
                return Json(new { success = false, message = "Game is not active" }, JsonRequestBehavior.AllowGet);
            }
            var UserId = User.Identity.GetUserId();
            var stages = new string[] { stage };
            Game2_Manager.SetUserScore(UserId, stages, EnableOverride: false); // Cannot override the score - once is set the done

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Game3
        [Authorize(Roles = "player")]
        public ActionResult Save_Game3_Score(bool completed, int attempts)
        {

            // Security - Check if Game is the currently active one - cannot set the score for a non active game
            var CurrentActiveGameKey = GameManager.GetActiveGameKey();
            if (CurrentActiveGameKey != Game1_Manager.GameKey)
            {
                return Json(new { success = false, message = "Game is not active" }, JsonRequestBehavior.AllowGet);
            }
            var UserId = User.Identity.GetUserId();
            Game3_Manager.SetUserScore(UserId, completed, attempts, EnableOverride: false); // Cannot override the score - once is set the done

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Get_Random_Game3_Solution()
        {
            var Rnd = new Random( DateTime.UtcNow.Second );

            var res = new int[4];
            for (int i = 0; i < res.Length; i++)
            {
                res[i] = Rnd.Next(1, 7);
            }

            return Json(res, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Game4_1

        [Authorize(Roles = "player")]
        public ActionResult Save_Game4_1_Score(string stage)
        {

            // Security - Check if Game is the currently active one - cannot set the score for a non active game
            var CurrentActiveGameKey = GameManager.GetActiveGameKey();
            if (CurrentActiveGameKey != Game1_Manager.GameKey)
            {
                return Json(new { success = false, message = "Game is not active" }, JsonRequestBehavior.AllowGet);
            }
            var UserId = User.Identity.GetUserId();
            var stages = new string[] { stage };
            Game4_1_Manager.SetUserScore(UserId, stages, EnableOverride: false); // Cannot override the score - once is set the done

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Game4_2

        [Authorize(Roles = "player")]
        public ActionResult Save_Game4_2Score(string stage)
        {

            // Security - Check if Game is the currently active one - cannot set the score for a non active game
            var CurrentActiveGameKey = GameManager.GetActiveGameKey();
            if (CurrentActiveGameKey != Game1_Manager.GameKey)
            {
                return Json(new { success = false, message = "Game is not active" }, JsonRequestBehavior.AllowGet);
            }
            var UserId = User.Identity.GetUserId();
            var stages = new string[] { stage };
            Game4_2_Manager.SetUserScore(UserId, stages, EnableOverride: false); // Cannot override the score - once is set the done

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Game4_1

        [Authorize(Roles = "player")]
        public ActionResult Save_Game4_3_Score(string stage)
        {

            // Security - Check if Game is the currently active one - cannot set the score for a non active game
            var CurrentActiveGameKey = GameManager.GetActiveGameKey();
            if (CurrentActiveGameKey != Game1_Manager.GameKey)
            {
                return Json(new { success = false, message = "Game is not active" }, JsonRequestBehavior.AllowGet);
            }
            var UserId = User.Identity.GetUserId();
            var stages = new string[] { stage };
            Game4_3_Manager.SetUserScore(UserId, stages, EnableOverride: false); // Cannot override the score - once is set the done

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Game5

        [Authorize(Roles = "player")]
        public ActionResult Save_Game5_Score(Dictionary<int,int> answers)
        {
            // Security - Check if Game is the currently active one - cannot set the score for a non active game
            var CurrentActiveGameKey = GameManager.GetActiveGameKey();
            if (CurrentActiveGameKey != Game6_Manager.GameKey)
            {
                return Json(new { success = false, message = "Game is not active" }, JsonRequestBehavior.AllowGet);
            }

            var UserId = User.Identity.GetUserId();
            Game5_Manager.HandleUserAnswers(UserId, answers, EnableOverride: false); // Cannot override the score - once is set the done

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CheckQuestion(int questionId, int answerIndex)
        {
            // Security - Check if Game is the currently active one - cannot set the score for a non active game
            var CurrentActiveGameKey = GameManager.GetActiveGameKey();
            if (CurrentActiveGameKey != Game6_Manager.GameKey)
            {
                return Json(new { success = false, message = "Game is not active" }, JsonRequestBehavior.AllowGet);
            }

            var UserId = User.Identity.GetUserId();
            var IsCorrect = Game5_Manager.CheckQuestionAnswer( questionId, answerIndex ); // Cannot override the score - once is set the done

            return Json(new { success = true, isCorrect = IsCorrect }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Game6

        [Authorize(Roles = "player")]
        public ActionResult Save_Game6_Score(int score)
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