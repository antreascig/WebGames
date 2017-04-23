﻿using System;
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
    public class GamesController : Controller
    {
        #region Views
        public ActionResult ActiveGame()
        {
            var activeGameInfo = GameManager.GetActiveGameInfo(User.Identity.GetUserId());

            if (Request.QueryString["showdemo"] == "true")
            {
                activeGameInfo.IsDemo = true;
                activeGameInfo.GameScore = 0;
                activeGameInfo.RemainingTime = 1 * 60; // 1 minute
            }

            return GetPage(activeGameInfo);
        }

        public ActionResult ActiveGameMap()
        {
            var activeGameInfo = GameManager.GetActiveGameInfo(User.Identity.GetUserId());

            return GetPage(activeGameInfo, "Map");
        }

        public ActionResult ActiveExplainer()
        {
            var activeGameInfo = GameManager.GetActiveGameInfo(User.Identity.GetUserId());

            return GetPage(activeGameInfo, "Explainer");
        }

        public ActionResult ActiveGameAfter()
        {
            var status = Request.QueryString["status"] ?? "";

            var activeGameInfo = GameManager.GetActiveGameInfo(User.Identity.GetUserId());

            if (activeGameInfo.ActiveGameDataModel != null)
            {
                var message = activeGameInfo.ActiveGameDataModel.Messages.ContainsKey(status) ? activeGameInfo.ActiveGameDataModel.Messages[status] : "";
                return View("ActiveGameAfter", new Dictionary<string, string>() { { "message", message } });
            }
            else
            {
                return View("NoActiveGame");
            }
        }
        #endregion

        #region Game3

        public ActionResult Get_Random_Game3_Solution()
        {
            var Rnd = new Random(DateTime.UtcNow.Second);

            var res = new int[4];
            for (int i = 0; i < res.Length; i++)
            {
                var num = 0;
                do
                {
                    num = Rnd.Next(1, 7);
                } while (!res.Contains(num));
                res[i] = num;
            }

            return Json(res, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Game5

        public ActionResult CheckQuestion(int questionId, int answerIndex)
        {
            var isDemoStr = Request.QueryString["isDemo"] ?? "false";
            var isDemo = false;
            bool.TryParse(isDemoStr, out isDemo);

            var UserId = User.Identity.GetUserId();
            // Security - Check if Game is the currently active one - cannot set the score for a non active game
            var ActiveGameDataModel = GameManager.GetActiveGameInfo(UserId).ActiveGameDataModel;
            if (!isDemo && (ActiveGameDataModel== null || ActiveGameDataModel.ActiveGameKey != GameKeys.Questions))
            {
                return Json(new { success = false, message = "Game is not active" }, JsonRequestBehavior.AllowGet);
            }

            var IsCorrect = Questions_Manager.CheckAndSaveQuestionAnswer(UserId, questionId, answerIndex); // Cannot override the score - once is set the done

            return Json(new { success = true, isCorrect = IsCorrect }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPlayerQuestions()
        {
            var res = new List<object>();
            try
            {
                res.AddRange(Questions_Manager.GetPlayerQuestions(User.Identity.GetUserId()).Select(q => new
                {
                    id = q.QuestionId,
                    question = q.QuestionText,
                    answer1 = q.Options[0],
                    answer2 = q.Options[1],
                    answer3 = q.Options[2],
                    answer4 = q.Options[3],
                }));
            }
            catch (Exception exc)
            {
                Logger.Log(exc);
            }
            return Json(res);
        }

        #endregion

        public ActionResult Save_Game_Score(int score, long timeStamp, int? level = 1)
        {
            var UserId = User.Identity.GetUserId();

            // Security - Check if Game is the currently active one - cannot set the score for a non active game
            var ActiveGameKey = GameManager.GetActiveGameInfo(UserId).ActiveGameDataModel.ActiveGameKey;
            if (ActiveGameKey == "")
            {
                return Json(new { success = false, message = "No Game is Active" }, JsonRequestBehavior.AllowGet);
            }

            GameManager.GameDict[ActiveGameKey].SM.SetUserScore(UserId, score, timeStamp, level.GetValueOrDefault(), EnableOverride: false); // Cannot override the score - once is set the done

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveGameTime(int remainingTime, long timestamp)
        {
            var UserId = User.Identity.GetUserId();
            ActivityManager.SyncPlayedTimeForToday(UserId, remainingTime, timestamp);
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetGameTime()
        {
            var UserId = User.Identity.GetUserId();
            var res = UserGameManager.GetUserRemainingTime(User.Identity.GetUserId());
            return Json(new { success = true, time = res }, JsonRequestBehavior.AllowGet);
        }

        // Helpers //
        private ActionResult GetPage(ActiveUserGameInfo gameInfo, string Page = "")
        {
            ActionResult ViewRes = null;
            // Active game was found
            if (gameInfo.ActiveGameDataModel != null)
            {
                // No remaining time
                if (gameInfo.RemainingTime <= 0)
                {
                    ViewRes = RedirectToAction("ActiveGameAfter", new { status = "outoftime" });
                } // no available levels
                else if (gameInfo.AvailableLevels > 0 && gameInfo.AvailableLevels < gameInfo.ActiveLevel)
                {
                    ViewRes = RedirectToAction("ActiveGameAfter", new { status = "outoftime" });
                }
                else
                {
                    string ViewPageToDisplay = gameInfo.Folder;
                    if (Page != "")
                    {
                        ViewPageToDisplay += $"/{Page}";
                    }
                    else // it's the game so use the PageFolder for the file
                    {
                        ViewPageToDisplay += $"/{gameInfo.Page}";

                        // If level is on its own page
                        if (gameInfo.LevelAsPage)
                        {
                            // use level specific page
                            ViewPageToDisplay += $"-{gameInfo.ActiveLevel}";
                        }
                    }

                    ViewRes = View(ViewPageToDisplay, gameInfo);
                }
            }
            else
            {
                // Nothing was found
                ViewRes = View("NoActiveGame");
            }

            return ViewRes;
        }
    }
}