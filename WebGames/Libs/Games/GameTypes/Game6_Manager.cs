using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebGames.Models;

namespace WebGames.Libs.Games.GameTypes
{
    public class Game6MetaData
    {
        //public double Multiplier { get; set; }
    }

    public class Game6_UserScore_Dto
    {
        public int GameId { get; set; }
        public string UserId { get; set; }
        public double Score { get; set; }
        public double Computed_Score { get; set; }
    }

    public class Game6_Manager
    {
        public static string GameKey = "Game6";

        //public static int Score_Limit { get; set; }

        public static void SetUserScore(string UserId, double Score, bool EnableOverride = false)
        {
            using (var db = ApplicationDbContext.Create())
            {
                var Entity = db.Game6_Scores.Find(UserId);
                if (Entity == null)
                {
                    Entity = new Models.Game6_UserScore()
                    {
                        UserId = UserId,
                        Score = Score,
                    };
                    db.Entry<Models.Game6_UserScore>(Entity);
                }
                else if (EnableOverride)
                {
                    Entity.Score = Score;
                }
                else
                {
                    return;
                }

                db.SaveChanges();
            }
        }

        public static Game6_UserScore_Dto GetUserScore(string UserId)
        {
            var res = new Game6_UserScore_Dto();
            using (var db = ApplicationDbContext.Create())
            {
                var ScoreEntity = (from gs in db.Game6_Scores where gs.UserId == UserId select gs).ToList().FirstOrDefault();
                if (ScoreEntity == null)
                {
                    ScoreEntity = new Models.Game6_UserScore()
                    {
                        UserId = UserId,
                        Score = 0
                    };
                }
                var Game = GameHelper.GetGame(GameManager.GameDict[GameKey], db);

                if (Game != null)
                {
                    res = GetUserScore(UserId, ScoreEntity.Score, Game.Multiplier);
                }
            }

            return res;
        }

        public static List<Game6_UserScore_Dto> GetUsersScore()
        {
            var res = new List<Game6_UserScore_Dto>();
            using (var db = ApplicationDbContext.Create())
            {
                var Game = GameHelper.GetGame(GameManager.GameDict[GameKey], db);
                if (Game != null)
                {
                    res = (from gs in db.Game6_Scores select gs).Select(gs => GetUserScore(gs.UserId, gs.Score, Game.Multiplier)).ToList();
                }
            }

            return res;
        }

        private static Game6_UserScore_Dto GetUserScore(string UserId, double Score, double Multiplier)
        {
            var res = new Game6_UserScore_Dto()
            {
                GameId = GameManager.GameDict[GameKey],
                UserId = UserId,
                Score = Score,
                Computed_Score = Multiplier * Score
            };
            return res;
        }

    }
}