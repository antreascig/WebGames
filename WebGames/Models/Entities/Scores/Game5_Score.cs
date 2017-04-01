using System.ComponentModel.DataAnnotations;

namespace WebGames.Models
{
    public class Game5_UserScore
    {
        [Key]
        public string UserId { get; set; }

        //public double Score { get; set; }

        public string Answers { get; set; }

        public int CorrectCount { get; set; }

        public int IncorrectCount { get; set; }

    }
}