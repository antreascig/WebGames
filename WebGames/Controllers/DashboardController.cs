using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using Mvc.JQuery.DataTables;
using WebGames.Libs;
using WebGames.Libs.Games;
using WebGames.Libs.Games.GameTypes;
using WebGames.Models;
using WebGames.Models.DatatableViewModels;
using WebGames.Models.ViewModels;

namespace WebGames.Controllers
{
    //[Authorize(Roles = "sysadmin,admin")]
    public class DashboardController : Controller
    {
        public DashboardController()
        {
        }

        public DashboardController(ApplicationUserManager userManager)
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

        // General Game Settings
        public ActionResult GetGameSettings()
        {
            try
            {
                var settings = GameManager.GetGameSettings();
                return Json(new { success = true, data = settings });
            }
            catch (Exception exc)
            {
                Logger.Log(exc);
                return Json(new { succsess = false, message = exc.Message });
            }
        }

        public ActionResult SetGameSettings(GameSettings model)
        {
            try
            {
                GameManager.SetGameSettings(model);
                return Json(new { succsess = true });
            }
            catch (Exception exc)
            {
                Logger.Log(exc);
                return Json(new { succsess = false, message = exc.Message });
            }
        }

        //Schedule Settings
        public ActionResult GetSchedule()
        {
            try
            {
                var Schedule = GameDayScheduleManager.GetSchedule();
                return Json(new { succsess = true, data = Schedule });
            }
            catch (Exception exc)
            {
                Logger.Log(exc.Message, LogType.ERROR);
                return Json(new { succsess = false, message = exc.Message });
            }
        }

        public ActionResult SaveSchedule(List<DayActiveGame> schedule)
        {
            try
            {
                GameDayScheduleManager.SaveSchedule(schedule);
                return Json(new { succsess = true });
            }
            catch (Exception exc)
            {
                Logger.Log(exc.Message, LogType.ERROR);
                return Json(new { succsess = false, message = exc.Message });
            }
        }
        
        // Game Questions
        public ActionResult GetGameQuestions()
        {
            try
            {
                var Questions = Game5_Manager.GetQuestions();
                return Json(new { succsess = true, data = Questions });
            }
            catch (Exception exc)
            {
                Logger.Log(exc);
                return Json(new { succsess = false, message = exc.Message });
            }
        }

        public ActionResult SaveGame5Questions(List<GameQuestionModel> questions)
        {
            try
            {
                Game5_Manager.SaveQuestions(questions);
                return Json(new { succsess = true });
            }
            catch (Exception exc)
            {
                Logger.Log(exc);
                return Json(new { succsess = false, message = exc.Message });
            }
        }

        // Scores
        public DataTablesResult GetScores(string GameKey, DataTablesParam dataTableParam)
        {

            var response = GameManager.GameDict[GameKey].SM.GetUsersScoresDT(dataTableParam);

            return response;
        }

        // Users
        public DataTablesResult GetUsers(DataTablesParam dataTableParam)
        {
            var response = DataTableManager.GetUsers(dataTableParam);

            return response;
        }

        public ActionResult GetUserDetails(string userId)
        {
            var user = UserManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }

            var res = new
            {
                success = true,
                Games = ScoreManager.GetUserTotalScores(userId)
            };
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveUserGameTokens(string userId, string gameTokensJSON)
        {
            try
            {
                var user = UserManager.FindByIdAsync(userId ?? "");
                if (user == null)
                {
                    return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                }

                var gameTokens = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, int>>(gameTokensJSON ?? "{}");

                if (!gameTokens.Any() || gameTokens.Any(g => !GameManager.GameDict.ContainsKey(g.Key)))
                {
                    return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                }

                foreach (var game in gameTokens)
                {
                    GameManager.GameDict[game.Key].SM.SetUserScore(userId, game.Value, true);
                }

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch( Exception exc)
            {
                Logger.Log(exc);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

    }

}