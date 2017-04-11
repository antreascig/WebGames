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
    [Authorize(Roles = "player")]
    public class GamesController: Controller
    {
        public ActionResult ActiveGame()
        {
            var PageToDisplay = "";

            var CurrentActiveGameKey = GameManager.GetActiveGameKey(User.Identity.GetUserId());

            if ((CurrentActiveGameKey ?? "") != "" && GameManager.GameDict.ContainsKey(CurrentActiveGameKey))
            {
                PageToDisplay = GameManager.GameDict[CurrentActiveGameKey].Page;
            }
            else
            {
                PageToDisplay = "NoActiveGame";
            }

            // FOR TESTING
            //PageToDisplay = "mastermind/Cosmoplay";
            //return new FilePathResult(PageToDisplay, "text/html");
            return View(PageToDisplay);
        }

        public ActionResult Save_Game_Score(int score)
        {
            var UserId = User.Identity.GetUserId();

            // Security - Check if Game is the currently active one - cannot set the score for a non active game
            var ActiveGameKey = GameManager.GetActiveGameKey(UserId);
            if (ActiveGameKey == "" )
            {
                return Json(new { success = false, message = "No Game is Active" }, JsonRequestBehavior.AllowGet);
            }

            GameManager.GameDict[ActiveGameKey].SM.SetUserScore(UserId, score, EnableOverride: false); // Cannot override the score - once is set the done

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        #region Game3

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

        #region Game5

        public ActionResult CheckQuestion(int questionId, int answerIndex)
        {
            var UserId = User.Identity.GetUserId();
            // Security - Check if Game is the currently active one - cannot set the score for a non active game
            var CurrentActiveGameKey = GameManager.GetActiveGameKey(UserId);
            if ( CurrentActiveGameKey != Game5_Manager.GameKey )
            { 
                return Json(new { success = false, message = "Game is not active" }, JsonRequestBehavior.AllowGet);
            }

            var IsCorrect = Game5_Manager.CheckAndSaveQuestionAnswer(UserId, questionId, answerIndex ); // Cannot override the score - once is set the done

            return Json(new { success = true, isCorrect = IsCorrect }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPlayerQuestions()
        {
            var res = new List<GameQuestionView>();
            try
            {
                res.AddRange(Game5_Manager.GetPlayerQuestions(User.Identity.GetUserId()));
            }
            catch(Exception exc)
            {
                Logger.Log(exc);
            }
            return Json(res);
        }

        #endregion

        public ActionResult SaveGameTime(int timeInSeconds, long timestamp)
        {
            var UserId = User.Identity.GetUserId();       
            ActivityManager.SyncPlayedTimeForToday(UserId, timeInSeconds, timestamp); // Cannot override the score - once is set the done
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

    }
}