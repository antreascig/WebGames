using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebGames.Libs;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using WebGames.Libs.Games;

namespace WebGames.Controllers
{
    [Authorize(Roles = "player")]
    public class DemoController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MasterMind()
        {
            ActiveUserGameInfo activeGameInfo = GetGameInfo(GameKeys.Mastermind);

            return GetPage(activeGameInfo);
        }

        public ActionResult Questions()
        {
            ActiveUserGameInfo activeGameInfo = GetGameInfo(GameKeys.Questions);

            return GetPage(activeGameInfo);
        }

        public ActionResult Whackamole()
        {
            ActiveUserGameInfo activeGameInfo = GetGameInfo(GameKeys.Whackamole);

            return GetPage(activeGameInfo);
        }

        public ActionResult Escape_1()
        {
            ActiveUserGameInfo activeGameInfo = GetGameInfo(GameKeys.Escape_1);

            return GetPage(activeGameInfo);
        }

        public ActionResult Escape_2()
        {
            ActiveUserGameInfo activeGameInfo = GetGameInfo(GameKeys.Escape_2);

            return GetPage(activeGameInfo);
        }

        public ActionResult Escape_3()
        {
            ActiveUserGameInfo activeGameInfo = GetGameInfo(GameKeys.Escape_3);

            return GetPage(activeGameInfo);
        }

        public ActionResult Juggler()
        {
            ActiveUserGameInfo activeGameInfo = GetGameInfo(GameKeys.Juggler);

            return GetPage(activeGameInfo);
        }

        public ActionResult Adespotabalakia()
        {
            ActiveUserGameInfo activeGameInfo = GetGameInfo(GameKeys.Adespotabalakia);

            return GetPage(activeGameInfo);
        }

        private ActiveUserGameInfo GetGameInfo(string GameKey)
        {
            var userId = User.Identity.GetUserId();
            var GameData = GameManager.GameDict[GameKey];

            var scoreData = GameData.SM.GetUserScore(userId);
            var activeGameInfo = new ActiveUserGameInfo()
            {
                ActiveGameDataModel = new ActiveGameData()
                {
                    ActiveGameKey = "",
                    Messages = new Dictionary<string, string>()
                },
                ActiveLevel = scoreData.Levels + 1,
                AvailableLevels = GameData.AvailableLevels,
                Folder = GameData.Folder,
                GameScore = scoreData.Score,
                IsDemo = false,
                LevelAsPage = GameData.LevelAsPage,
                Page = GameData.Page,
                RemainingTime = UserGameManager.GetUserRemainingTime(userId).RemainingTimeInSeconds
            };
            return activeGameInfo;
        }

        public ActionResult ActiveGameAfter()
        {
            var message = "Τέλος του Demo";
            return View("ActiveGameAfter", new Dictionary<string, string>() { { "message", message } });

        }

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