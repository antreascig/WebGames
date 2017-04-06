using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebGames.Models
{
    public class Game3_UserScore
    {
        [Key]
        public string UserId { get; set; }

        public bool Completed { get; set; }

        public int Attempts { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
    }
}