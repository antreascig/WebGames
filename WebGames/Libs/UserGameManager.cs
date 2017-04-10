using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebGames.Helpers;
using WebGames.Libs.Games;
using WebGames.Models;
using WebGames.Models.ViewModels;

namespace WebGames.Libs
{
    public class UserGameManager
    {
        const int TIME_LIMIT_PER_DAY = 5 * 60; // 300 seconds / 5 minutes

        public static UserGameInfo GetUserGameInfo(string UserId)
        {
            var Today = DateHelper.GetGreekDate(DateTime.UtcNow, onlyDate: true);

            // Get Remaining Play time for today
            var gameTime = ActivityManager.GetGameTime(UserId, Today);
            var RemainingTime = TIME_LIMIT_PER_DAY - gameTime.timeInSeconds;
            if (RemainingTime < 0) RemainingTime = 0;

            var res = new UserGameInfo()
            {
                RemainingTimeInSeconds = RemainingTime,
                timeStamp = gameTime.timeStamp
            };
            return res;
        }
    }
}