using WebGames.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebGames.Models
{
    public class Game6_UserScore
    {
        [Key]
        public string UserId { get; set; }

        public double Score { get; set; }
    }
}