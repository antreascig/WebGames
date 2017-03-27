using WebGames.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebGames.Models
{
    public class UserGameScore
    {
        [Column(Order = 0), Key]
        public int GameId{ get; set; }

        [Column(Order = 1), Key]
        public string UserId { get; set; }

        public double Score { get; set; }


        // FKs
        [ForeignKey("GameId")]
        public GameModel Game { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
    }
}