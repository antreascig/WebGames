using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebGames.Models
{
    public class Game1_UserScore
    {
        [Key]
        public string UserId { get; set; }

        public double Score { get; set; }
    }
}