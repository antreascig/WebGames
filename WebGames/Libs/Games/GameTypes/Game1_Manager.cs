using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebGames.Models;

namespace WebGames.Libs.Games.GameTypes
{
    public class Game1_MetaData
    {
        //public double Multiplier { get; set; }
    }

    public class Game1_UserScore_Dto
    {
        public int GameId { get; set; }
        public string UserId { get; set; }
        public double Score { get; set; }
        public double Computed_Score { get; set; }
    }

    public class Game1_Manager
    {
        public static string GameKey = "Game1";

        //public static int Score_Limit { get; set; }

        public static int GameId
        {
            get
            {
                return GameManager.GameDict[GameKey];
            }
        }

        public static void SetUserScore(string UserId, double Score, bool EnableOverride = false)
        {
            using (var db = ApplicationDbContext.Create())
            {
                var Entity = db.Game1_Scores.Find(UserId);
                if (Entity == null)
                {
                    Entity = new Models.Game1_UserScore()
                    {
                        UserId = UserId,
                        Score = Score,
                    };
                    db.Entry<Models.Game1_UserScore>(Entity);
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

        public static Game1_UserScore_Dto GetUserScore(string UserId)
        {
            var res = new Game1_UserScore_Dto();
            using (var db = ApplicationDbContext.Create())
            {
                var ScoreEntity = (from gs in db.Game1_Scores where gs.UserId == UserId select gs).ToList().FirstOrDefault();
                if (ScoreEntity == null)
                {
                    ScoreEntity = new Models.Game1_UserScore()
                    {
                        UserId = UserId,
                        Score = 0
                    };
                }
                var Game = GameHelper.GetGame(GameId, db);

                if (Game != null)
                {
                    res = GenerateUserScore(UserId, ScoreEntity.Score, Game.Multiplier);
                }
            }

            return res;
        }

        public static List<Game1_UserScore_Dto> GetUsersScore()
        {
            var res = new List<Game1_UserScore_Dto>();
            using (var db = ApplicationDbContext.Create())
            {
                var Game = GameHelper.GetGame(GameId, db);
                if (Game != null)
                {
                    res = (from gs in db.Game1_Scores select gs).Select(gs => GenerateUserScore(gs.UserId, gs.Score, Game.Multiplier)).ToList();
                }
            }

            return res;
        }

        private static Game1_UserScore_Dto GenerateUserScore(string UserId, double Score, double Multiplier)
        {
            var res = new Game1_UserScore_Dto()
            {
                GameId = GameId,
                UserId = UserId,
                Score = Score,
                Computed_Score = Multiplier * Score
            };
            return res;
        }

    }
}