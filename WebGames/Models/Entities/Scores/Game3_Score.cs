using System.ComponentModel.DataAnnotations;

namespace WebGames.Models
{
    public class Game3_UserScore
    {
        [Key]
        public string UserId { get; set; }

        public bool Completed { get; set; }

        public string Stages { get; set; }
    }
}