using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebGames.Models.ViewModels
{
    public class UserGameInfo
    {
        public string UserName { get; set; }

        public string Avatar { get; set; }

        public int RemainingTimeInSeconds { get; set; }
    }
}