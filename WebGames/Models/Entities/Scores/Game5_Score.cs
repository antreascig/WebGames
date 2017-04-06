using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
    }
}