using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebGames.Models
{
    public class Game2_UserScore
    {
        [Key]
        public string UserId { get; set; }

        public string Stages { get; set; }

        public int StagesCount { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
    }
}