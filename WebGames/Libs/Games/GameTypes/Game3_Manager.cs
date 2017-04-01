using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebGames.Helpers;
using WebGames.Models;

namespace WebGames.Libs.Games.GameTypes
{
    public class Game3_MetaData
    {
        //public double Multiplier { get; set; }
    }

    public class Game3_UserScore_Dto
    {
        public int GameId { get; set; }
        public string UserId { get; set; }
        public int Attempts { get; set; }
        public double Computed_Score { get; set; }
    }

    public class Game3_Manager
    {
        public static string GameKey = "Game3";

        private static double CompletionScore = 1;

        //public static int Score_Limit { get; set; }

        public static void SetUserScore(string UserId, bool Completed, int Attempts, bool EnableOverride = false)
        {
            using (var db = ApplicationDbContext.Create())
            {
                var Entity = db.Game3_Scores.Find(UserId);
                if (Entity == null)
                {
                    Entity = new Models.Game3_UserScore()
                    {
                        UserId = UserId,
                        Completed = false,
                        Attempts = 0
                    };
                    db.Entry<Models.Game3_UserScore>(Entity);
                }
                else if (EnableOverride)
                {
                    Entity.Completed = Completed;
                    Entity.Attempts = Attempts;
                }
                else
                {
                    return;
                }

                db.SaveChanges();
            }
        }

        public static Game3_UserScore_Dto GetUserScore(string UserId)
        {
            var res = new Game3_UserScore_Dto();
            using (var db = ApplicationDbContext.Create())
            {
                var ScoreEntity = (from gs in db.Game3_Scores where gs.UserId == UserId select gs).ToList().FirstOrDefault();
                if (ScoreEntity == null)
                {
                    ScoreEntity = new Models.Game3_UserScore()
                    {
                        UserId = UserId,
                        Completed = false,
                        Attempts = 0
                    };
                }

                var Game = GameHelper.GetGame(GameManager.GameDict[GameKey], db);

                if (Game != null)
                {
                    res = GetUserScore(UserId, ScoreEntity.Completed, ScoreEntity.Attempts, Game.Multiplier);
                }
            }

            return res;
        }

        public static List<Game3_UserScore_Dto> GetUsersScore()
        {
            var res = new List<Game3_UserScore_Dto>();
            using (var db = ApplicationDbContext.Create())
            {
                var Game = GameHelper.GetGame(GameManager.GameDict[GameKey], db);
                if (Game != null)
                {
                    res = (from gs in db.Game3_Scores select gs).Select(gs => GetUserScore(gs.UserId, gs.Completed, gs.Attempts, Game.Multiplier)).ToList();
                }
            }

            return res;
        }

        private static Game3_UserScore_Dto GetUserScore(string UserId, bool Completed, int Attempts, double Multiplier)
        {
            var res = new Game3_UserScore_Dto()
            {
                GameId = GameManager.GameDict[GameKey],
                UserId = UserId,
                Attempts = Attempts,
                Computed_Score = Multiplier * ( CompletionScore / Attempts)
            };
            return res;
        }
    }
}