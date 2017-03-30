using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebGames.Models
{
    public class DayActiveGame
    {
        [Column(TypeName = "DateTime2"), Key]
        public DateTime Day { get; set; }

        public string GameKey { get; set; }
    }
}