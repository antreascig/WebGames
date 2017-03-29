using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebGames.Helpers;
using WebGames.Models;
using WebGames.Models.Dtos;

namespace WebGames.Libs.Games
{
    public class ScoreManager
    {

        public static void SetUserScore(int GameId, string UserId, int score, bool EnableOverride = false)
        {
            using (var db = ApplicationDbContext.Create())
            {
                var Entity = db.Scores.Find(GameId, UserId);
                if (Entity == null)
                {
                    Entity = new UserGameScore()
                    {
                        GameId = GameId,
                        UserId = UserId,
                        Score = score
                    };
                    db.Entry<UserGameScore>(Entity);
                }
                else if (EnableOverride)
                {
                    Entity.Score = score;
                }
                else
                {
                    return;
                }

                db.SaveChanges();
            }
        }

        public static UserScoreDto GetUserScore(int GameId, string UserId)
        {
            var res = new UserScoreDto();
            using (var db = ApplicationDbContext.Create())
            {
                var score = db.Scores.Find(GameId, UserId);
                res = GetComputedUserScore(score, GameId, UserId);
            }

            return res;
        }

        public static List<UserScoreDto> GetUsersScores(int GameId, int? UserLimit)
        {
            var res = new List<UserScoreDto>();

            if (!GameManager.GameDict.ContainsKey(GameId)) return res;

            using (var db = ApplicationDbContext.Create())
            {
                var q = db.Scores.Where(s => s.GameId == GameId);
                if (UserLimit.HasValue)
                {
                    q.Take(UserLimit.Value);
                }

                res = q.ToList()
                       .Select(score => GetComputedUserScore(score, score.GameId, score.UserId))
                       .ToList();
            }
            return res;
        }

        private static UserScoreDto GetComputedUserScore(UserGameScore UserScore, int GameId, string UserId)
        {
            var res = new UserScoreDto() { GameId = GameId, UserId = UserId, Score = 0 };
            if (UserScore == null) return res;

            res.CopyObjectFrom(UserScore);

            // Check if the Game exists in the GameDict
            if (!GameManager.GameDict.ContainsKey(UserScore.GameId)) return res;

            var GameStrategy = GameManager.GameDict[UserScore.GameId];

            res.Computed_Score = GameStrategy.ComputeScore(UserScore);

            return res;
        }
    }
}