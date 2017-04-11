using WebGames.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebGames.Models
{
    public class Game6_User_Group
    {
        public string UserId { get; set; }

        public int GroupNumber { get; set; }
    }

    public class Game6_UserScore : A_UserScore
    {

    }
}