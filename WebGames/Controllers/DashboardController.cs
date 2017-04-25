﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Mvc.JQuery.DataTables;
using WebGames.Libs;
using WebGames.Libs.Games;
using WebGames.Libs.Games.Games;
using WebGames.Libs.Games.GameTypes;
using WebGames.Models;
using WebGames.Models.DatatableViewModels;
using WebGames.Models.ViewModels;

namespace WebGames.Controllers
{
    public class DashboardController : Controller
    {
        public DashboardController()
        {
        }

        public DashboardController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
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

        private ApplicationSignInManager _signInManager;

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set { _signInManager = value; }
        }

        #region Views

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View("Login");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if ((returnUrl ?? "") == "") returnUrl = "/Dashboard/Index";

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Require the user to have a confirmed email before they can log on.
            // var user = await UserManager.FindByNameAsync(model.Email);
            var user = UserManager.Find(model.UserName, model.Password);
            if (user != null)
            {
                var isADMIN = UserManager.IsInRole(user.Id, "sysadmin") || UserManager.IsInRole(user.Id, "admin");

                if (!isADMIN)
                {
                    AddErrors(new IdentityResult("Ο λογαριασμός σου δεν αντιστοιχεί σε admin."));
                    return View(model);
                }
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "DashBoard");
            }
            ViewBag.Link = TempData["ViewBagLink"];

            if (User.IsInRole("player"))
            {
                return RedirectToAction("Index", "Home");
            }

            var model = new DashBoardIndexModel()
            {
                ActiveGame = "-",
                NumberOfPlayers = -1,
                ScheduleDays = new List<string>(),
                ScheduleGames = new List<string>()
            };

            try
            {
                var activeGameData = GameDayScheduleManager.GetActiveGame(DateTime.UtcNow);
                if (activeGameData != null && GameManager.GameDict.ContainsKey(activeGameData.ActiveGameKey))
                {
                    model.ActiveGame = GameManager.GameDict[activeGameData.ActiveGameKey].Name;
                }

                using (var db = ApplicationDbContext.Create())
                {
                    var PlayerRoleId = SecurityManager.Roles.FirstOrDefault(r => r.Name == "player").Id;
                    var playersCount = (from user in db.Users where user.Roles.Any(r => r.RoleId == PlayerRoleId) select user).Count();
                    model.NumberOfPlayers = playersCount;
                }

                var daysToCheckForActiveGame = 5;
                var i = -1;
                var startingDate = DateTime.UtcNow;
                var GameName = "";
                do
                {
                    var Date = startingDate.ToString("yyyy-MM-dd");
                    var activeGameDataModel = GameDayScheduleManager.GetActiveGame(startingDate);
                    var ActiveGameKey = activeGameDataModel != null ? activeGameDataModel.ActiveGameKey : "";
                    if (GameManager.GameDict.ContainsKey(ActiveGameKey ?? ""))
                    {
                        GameName = GameManager.GameDict[ActiveGameKey].Name;
                    }
                    else
                    {
                        GameName = "-";
                    }
                    model.ScheduleDays.Add(Date);
                    model.ScheduleGames.Add(GameName);
                    i++;
                    startingDate = startingDate.AddDays(1);
                } while (i < daysToCheckForActiveGame);
            }
            catch (Exception exc)
            {
                Logger.Log(exc);
            }

            return View(model);
        }

        [Authorize(Roles = "sysadmin,admin")]
        public ActionResult Users()
        {
            return View();
        }

        [Authorize(Roles = "sysadmin,admin")]
        public ActionResult Scores()
        {
            return View();
        }

        [Authorize(Roles = "sysadmin,admin")]
        public ActionResult GeneralSettings()
        {
            return View();
        }

        [Authorize(Roles = "sysadmin,admin")]
        public ActionResult QuestionsSettings()
        {
            return View();
        }

        [Authorize(Roles = "sysadmin,admin")]
        public ActionResult GroupsSettings()
        {
            return View();
        }


        [Authorize(Roles = "sysadmin,admin")]
        public ActionResult Schedule()
        {
            return View();
        }

        //[HttpGet]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Dashboard");
        }

        #endregion

        // General Game Settings
        [Authorize(Roles = "sysadmin,admin")]
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
                return Json(new { success = false, message = exc.Message });
            }
        }

        [Authorize(Roles = "sysadmin,admin")]
        public ActionResult SetGameSettings(GameSettings model)
        {
            try
            {
                GameManager.SetGameSettings(model);
                return Json(new { success = true });
            }
            catch (Exception exc)
            {
                Logger.Log(exc);
                return Json(new { success = false, message = exc.Message });
            }
        }

        //Schedule Settings
        [Authorize(Roles = "sysadmin,admin")]
        public ActionResult GetSchedule()
        {
            try
            {
                var Schedule = GameDayScheduleManager.GetSchedule();
                return Json(new { success = true, data = Schedule });
            }
            catch (Exception exc)
            {
                Logger.Log(exc.Message, LogType.ERROR);
                return Json(new { success = false, message = exc.Message });
            }
        }

        [Authorize(Roles = "sysadmin,admin")]
        public ActionResult SaveSchedule(string scheduleJSON)
        {
            try
            {
                scheduleJSON = Server.UrlDecode(scheduleJSON ?? "");

                var schedule = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DayActiveGame>>(scheduleJSON ?? "{}");

                GameDayScheduleManager.SaveSchedule(schedule);
                return Json(new { success = true });
            }
            catch (Exception exc)
            {
                Logger.Log(exc.Message, LogType.ERROR);
                return Json(new { success = false, message = exc.Message });
            }
        }

        // Game Questions
        [Authorize(Roles = "sysadmin,admin")]
        public ActionResult GetGameQuestions()
        {
            try
            {
                var Questions = Questions_Manager.GetQuestions();
                return Json(new { success = true, data = Questions }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exc)
            {
                Logger.Log(exc);
                return Json(new { success = false, message = exc.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize(Roles = "sysadmin,admin")]
        public ActionResult SaveGame5Questions(List<GameQuestionModel> questions)
        {
            try
            {
                Questions_Manager.SaveQuestions(questions);
                return Json(new { success = true });
            }
            catch (Exception exc)
            {
                Logger.Log(exc);
                return Json(new { success = false, message = exc.Message });
            }
        }

        // Scores
        [Authorize(Roles = "sysadmin,admin")]
        public DataTablesResult GetScores(string GameKey, DataTablesParam dataTableParam)
        {

            var response = GameManager.GameDict[GameKey].SM.GetUsersScoresDT(dataTableParam);

            return response;
        }

        [Authorize(Roles = "sysadmin,admin")]
        public DataTablesResult GetGroupScores(string group, DataTablesParam dataTableParam)
        {

            var groups = Group_Manager.GetGroups();

            int groupNumber;
            if (int.TryParse(group, out groupNumber))
            {
                var data = groups.Where(g => g.Key == groupNumber).SelectMany(g => g.Value.Select(u => new GroupViewModel()
                {
                    GroupNumber = g.Key,
                    UserId = u.UserId,
                    User_FullName = u.User_FullName,
                    Score = u.Score,
                    Controls = ""
                })).AsQueryable();

                return DataTablesResult.Create<GroupViewModel>(data, dataTableParam);
            }
            else
            {
                var data = groups.Select(g => new GroupsViewModel()
                {
                    GroupNumber = g.Key,
                    Score = g.Value.Sum(u => u.Score),
                    Controls = ""
                }).ToList();

                // test data 
                //for (var i=1; i <= 12; i++)
                //{
                //    data.Add(new GroupsViewModel() { GroupNumber = i, Score = i * 10, Controls = "" });
                //}

                return DataTablesResult.Create<GroupsViewModel>(data.AsQueryable(), dataTableParam);
            }
        }

        // Users
        [Authorize(Roles = "sysadmin,admin")]
        public DataTablesResult GetUsers(DataTablesParam dataTableParam)
        {
            var response = DataTableManager.GetUsers(dataTableParam);

            return response;
        }

        [Authorize(Roles = "sysadmin,admin")]
        public ActionResult GetUserDetails(string userId)
        {
            var user = UserManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }

            var PlayTimeToday = 0;
            var time = ActivityManager.GetGameTime(userId, DateTime.UtcNow);
            if (time != null)
            {
                PlayTimeToday = time.timeInSeconds;
            }

            var res = new
            {
                success = true,
                Games = ScoreManager.GetUserTotalScores(userId),
                PlayTimeToday = PlayTimeToday
            };
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "sysadmin,admin")]
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
                long timeStamp = (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds / 1);

                foreach (var game in gameTokens)
                {
                    GameManager.GameDict[game.Key].SM.SetUserScore(userId, game.Value, timeStamp, 1, true);
                }

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exc)
            {
                Logger.Log(exc);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize(Roles = "sysadmin,admin")]
        public ActionResult ResetGameTime(string userId)
        {
            try
            {
                var user = UserManager.FindByIdAsync(userId ?? "");
                if (user == null)
                {
                    return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                }

                // calculate timestamp yourDateObject.getTime()
                long timeStamp = (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds / 1);

                ActivityManager.SavePlayTime(userId, DateTime.UtcNow, 0, timeStamp, true);

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exc)
            {
                Logger.Log(exc);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        // Group Settings
        [Authorize(Roles = "sysadmin,admin")]
        public ActionResult GetSavedGroups()
        {
            try
            {
                var user_groups = Group_Manager.GetGroups().SelectMany(g => g.Value.Select(u => new UserGroupVM()
                {
                    Rank = -1,
                    UserId = u.UserId,
                    User_FullName = u.User_FullName,
                    Group = g.Key,
                    Controls = ""
                }));
                return Json(new { success = true, user_groups = user_groups }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exc)
            {
                Logger.Log(exc);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize(Roles = "sysadmin,admin")]
        public ActionResult GetRankedPlayers()
        {
            try
            {
                var ranked_groups = Group_Manager.GetRankingsBeforeGroups().Take(144).ToList();

                return Json(new { success = true, ranked_groups = ranked_groups }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exc)
            {
                Logger.Log(exc);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize(Roles = "sysadmin,admin")]
        public DataTablesResult GetRankedPlayersDT(DataTablesParam dataTableParam)
        {
            var ranked_groups = Group_Manager.GetRankingsBeforeGroups().AsQueryable();

            return DataTablesResult.Create<UserGroupVM>(ranked_groups, dataTableParam);
        }

        public ActionResult SaveGroups( string user_groupsJSON)
        {
            try
            {
                List<User_Group> user_groups = Newtonsoft.Json.JsonConvert.DeserializeObject<List<User_Group>>(user_groupsJSON ?? "[]");

                if (user_groups == null)
                    return Json(new { success = false }, JsonRequestBehavior.AllowGet);

                Group_Manager.SetGroups(user_groups);

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exc)
            {
                Logger.Log(exc);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        // HELPERS

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
    }

}