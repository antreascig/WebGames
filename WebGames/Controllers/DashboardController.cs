using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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

            var response = GameManager.GameDict[GameKey].SM.GetUserScores(dataTableParam);

            return response;
        }

        // Scores
        public DataTablesResult GetUsers(DataTablesParam dataTableParam)
        {
            var response = DataTableManager.GetUsers(dataTableParam);

            return response;
        }
    }

}