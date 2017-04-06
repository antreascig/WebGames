using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebGames.Helpers;
using WebGames.Models;

namespace WebGames.Libs.Games.GameTypes
{
    public class Game4_1_MetaData
    {
        //public double Multiplier { get; set; }
    }

    public class Game4_1_UserScore_Dto
    {
        public int GameId { get; set; }
        public string UserId { get; set; }
        public string Stages { get; set; }
        public double Computed_Score { get; set; }
    }

    public class Game4_1_Manager
    {
        public static string GameKey = "Game4_1";

        //private static double StageScore = 1;

        public static int GameId
        {
            get
            {
                return GameManager.GameDict[GameKey];
            }
        }

        //public static int Score_Limit { get; set; }

        public static void SetUserScore(string UserId, string[] Stages, bool EnableOverride = false)
        {
            Stages = Stages ?? new string[] { };
            using (var db = ApplicationDbContext.Create())
            {
                var Entity = db.Game4_1_Scores.Find(UserId);
                if (Entity == null)
                {
                    Entity = new Models.Game4_1_UserScore()
                    {
                        UserId = UserId,
                        StagesCount = Stages.Length,
                        Stages = string.Join(",", Stages)
                    };
                    db.Entry<Models.Game4_1_UserScore>(Entity);
                }
                else if (EnableOverride)
                {
                    Entity.StagesCount = Stages.Length;
                    Entity.Stages = string.Join(",", Stages);
                }
                else
                {
                    // Check if stage was already set
                    var ExistingStages = (Entity.Stages ?? "").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                    if (Stages.Except(ExistingStages).Count() == 0) return;

                    var finalList = ExistingStages.Union(Stages);
                    // if not then set it accordingly
                    Entity.Stages = string.Join(",", finalList);
                    Entity.StagesCount = finalList.Count();
                }

                db.SaveChanges();
            }
        }

        public static Game4_1_UserScore_Dto GetUserScore(string UserId)
        {
            var res = new Game4_1_UserScore_Dto();
            using (var db = ApplicationDbContext.Create())
            {
                var ScoreEntity = (from gs in db.Game4_1_Scores where gs.UserId == UserId select gs).ToList().FirstOrDefault();
                if (ScoreEntity == null)
                {
                    ScoreEntity = new Models.Game4_1_UserScore()
                    {
                        UserId = UserId,
                        Stages = "",
                        StagesCount = 0
                    };
                }

                var Game = GameHelper.GetGame(GameId, db);

                if (Game != null)
                {
                    res = GenerateUserScore(UserId, ScoreEntity.Stages, ScoreEntity.StagesCount, Game.Multiplier);
                }
            }

            return res;
        }

        public static List<Game4_1_UserScore_Dto> GetUsersScore()
        {
            var res = new List<Game4_1_UserScore_Dto>();
            using (var db = ApplicationDbContext.Create())
            {
                var Game = GameHelper.GetGame(GameId, db);
                if (Game != null)
                {
                    res = (from gs in db.Game4_1_Scores select gs).Select(gs => GenerateUserScore(gs.UserId, gs.Stages, gs.StagesCount, Game.Multiplier)).ToList();
                }
            }

            return res;
        }

        private static Game4_1_UserScore_Dto GenerateUserScore(string UserId, string stages, int StagesCount, double Multiplier)
        {
            //var numberOfStagesCompleted = (stages ?? "").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Length;
            var res = new Game4_1_UserScore_Dto()
            {
                GameId = GameId,
                UserId = UserId,
                Stages = stages,
                Computed_Score = Multiplier * StagesCount
            };
            return res;
        }
    }
}