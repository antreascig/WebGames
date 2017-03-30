using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebGames.Helpers;
using WebGames.Models;

namespace WebGames.Libs.Games.GameTypes
{
    public class Game2MetaData
    {
        //public double Multiplier { get; set; }
    }

    public class Game2_UserScore_Dto
    {
        public int GameId { get; set; }
        public string UserId { get; set; }
        public string Stages { get; set; }
        public double Computed_Score { get; set; }
    }

    public class Game2_Manager
    {
        public static string GameKey = "Game2";

        private static double StageScore = 1;

        //public static int Score_Limit { get; set; }

        public static void SetUserScore(string UserId, double Score, string Stage, bool EnableOverride = false)
        {
            using (var db = ApplicationDbContext.Create())
            {
                var Entity = db.Game2_Scores.Find(UserId);
                if (Entity == null)
                {
                    Entity = new Models.Game2_UserScore()
                    {
                        UserId = UserId,
                        Score = Score,
                        Stages = Stage
                    };
                    db.Entry<Models.Game2_UserScore>(Entity);
                }
                else if (EnableOverride)
                {
                    Entity.Score = Score;
                    Entity.Stages = Stage;
                }
                else
                {
                    // Check if stage was already set
                    var ExistingStages = (Entity.Stages ?? "").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if ( ExistingStages.Contains(Stage)) return;
                    // if not then set it accordingly
                    Entity.Stages.JoinWith(",", Stage);
                    Entity.Score += Score;                    
                }

                db.SaveChanges();
            }
        }

        public static Game2_UserScore_Dto GetUserScore(string UserId)
        {
            var res = new Game2_UserScore_Dto();
            using (var db = ApplicationDbContext.Create())
            {
                var ScoreEntity = (from gs in db.Game2_Scores where gs.UserId == UserId select gs).ToList().FirstOrDefault();
                if (ScoreEntity == null)
                {
                    ScoreEntity = new Models.Game2_UserScore()
                    {
                        UserId = UserId,
                        Score = 0,
                        Stages = ""
                    };
                }

                var Game = GameHelper.GetGame(GameManager.GameDict[GameKey], db);

                if (Game != null)
                {
                    res = GetUserScore(UserId, ScoreEntity.Stages, ScoreEntity.Score, Game.Multiplier);
                }
            }

            return res;
        }

        public static List<Game2_UserScore_Dto> GetUsersScore()
        {
            var res = new List<Game2_UserScore_Dto>();
            using (var db = ApplicationDbContext.Create())
            {
                var Game = GameHelper.GetGame(GameManager.GameDict[GameKey], db);
                if (Game != null)
                {
                    res = (from gs in db.Game2_Scores select gs).Select(gs => GetUserScore(gs.UserId, gs.Stages, gs.Score, Game.Multiplier)).ToList();
                }
            }

            return res;
        }

        private static Game2_UserScore_Dto GetUserScore(string UserId, string stages, double Score, double Multiplier)
        {
            var numberOfStagesCompleted = (stages ?? "").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Length;
            var res = new Game2_UserScore_Dto()
            {
                GameId = GameManager.GameDict[GameKey],
                UserId = UserId,
                Stages = stages,
                Computed_Score = Multiplier * numberOfStagesCompleted
            };
            return res;
        }
    }
}