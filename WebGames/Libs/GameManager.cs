using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebGames.Models;
using WebGames.Models.Dtos;

namespace WebGames.Libs
{
    public class GameManager
    {

        public static void Init()
        {
            // initialize the games
        }

        public static UserScore GetUserScore(int GameId, string UserId)
        {
            throw new NotImplementedException();
        }

        public static List<UserScore> GetUserScores(int GameId, int? UserLimit)
        {
            var scores = new List<UserScore>();
            using (var db = ApplicationDbContext.Create() )
            {
                var q = db.Scores.Where(s => s.GameId == GameId);
                if (UserLimit.HasValue)
                {
                    q.Take(UserLimit.Value);
                }
                scores = q.ToList().Select(s => new UserScore()
                {
                    GameId = s.GameId,
                    GameName = s.Game.Name,
                    UserId = s.UserId,
                    UserName = s.User.Name,
                    Score = s.Score
                }).ToList();
            }

            return scores;
        }

    }
}