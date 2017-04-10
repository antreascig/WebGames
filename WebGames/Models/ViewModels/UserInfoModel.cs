﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebGames.Libs.Games;

namespace WebGames.Models.ViewModels
{
    public class UserGameInfo
    {
        public int RemainingTimeInSeconds { get; set; }

        public long timeStamp { get; internal set; }
    }


    public class UserScoreInfo
    {
        public List<UserScore> GameScores { get; set; }
    }

}