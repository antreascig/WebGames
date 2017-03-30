using System.ComponentModel.DataAnnotations;

namespace WebGames.Models
{
    public class Game4_3_UserScore
    {
        [Key]
        public string UserId { get; set; }

        //public double Score { get; set; }

        public string Stages { get; set; }
    }
}