using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebGames.Models.Dtos
{
    public class UserScore
    {
        public int GameId { get; set; }

        public string UserId { get; set; }

        public double Score { get; set; }

        public string UserName { get; set; }

        public string GameName { get; set; }
    }
}