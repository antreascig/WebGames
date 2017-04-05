using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebGames.Models;

namespace WebGames.Libs.Games.GameTypes
{
    //public class GameQuestionView
    //{
    //    public int QuestionId { get; set; }
    //    public int QuestionText { get; set; }
    //    public List<string> Options { get; set; }
    //    public int AnswerIndex { get; set; }
    //}

    public class GameQuestionModel
    {
        public int QuestionId { get; set; }
        public bool Active { get; set; }
        public int QuestionText { get; set; }
        public List<string> Options { get; set; }
        public int AnswerIndex { get; set; }
    }

    public class Game5_MetaData
    {
        public Dictionary<int, GameQuestionModel> Questions { get; set; }
    }

    public class Game5_UserScore_Dto
    {
        public int GameId { get; set; }
        public string UserId { get; set; }
        public string Answers { get; set; } // Dictionary<int, int>
        public double Computed_Score { get; set; }
    }

    public class Game5_Manager
    {
        public static string GameKey = "Game5";

        public static int GameId {
            get
            {
                return GameManager.GameDict[GameKey];
            }
        }

        //public static int Score_Limit { get; set; }
        public static List<GameQuestionModel> GetQuestions(int NumberOfQuestions)
        {
            var res = new List<GameQuestionModel>();
            try
            {
                var GameMetadata = (Game5_MetaData)GameHelper.GetGameMetaData(GameId, typeof(Game5_MetaData));
                if (GameMetadata != null && GameMetadata.Questions != null)
                {
                    res.AddRange(GameMetadata.Questions.Where(q => q.Value.Active).Select(q => q.Value));
                }
            }
            catch(Exception exc)
            {
                Logger.Log(exc.Message, LogType.ERROR);
            }
            return res;
        }

        public static bool CheckQuestionAnswer(int QuestionId, int AnswerIndex, GameModel Game = null)
        {
            try
            {
                var GameMetadata = (Game5_MetaData)GameHelper.GetGameMetaData(GameId, typeof(Game5_MetaData));
                // Check that is the correct question
                if (GameMetadata == null
                    || GameMetadata.Questions == null
                    || !GameMetadata.Questions.ContainsKey(QuestionId)
                    || GameMetadata.Questions[QuestionId].QuestionId != QuestionId
                    || GameMetadata.Questions[QuestionId].AnswerIndex != AnswerIndex)
                {
                    return false;
                }
                return true;
            }
            catch (Exception exc)
            {
                Logger.Log(exc.Message, LogType.ERROR);
                return false;
            }
        }

        public static void HandleUserAnswers(string UserId, Dictionary<int, int> Answers, bool EnableOverride = false)
        {
            try
            {
                var GameMetadata = (Game5_MetaData)GameHelper.GetGameMetaData(GameId, typeof(Game5_MetaData));
                if (GameMetadata == null || GameMetadata.Questions == null) return;
                int Correct = 0;
                int Incorrect = 0;
                foreach ( var answer in Answers)
                {
                    if (GameMetadata.Questions.ContainsKey(answer.Key))
                    {
                        if ( GameMetadata.Questions[answer.Key].AnswerIndex == answer.Value)
                        {
                            Correct++;
                        }
                        else
                        {
                            Incorrect++;
                        }
                    }
                }
                SetUserScore(UserId, Answers, Correct, Incorrect, EnableOverride);
            }
            catch (Exception exc)
            {
                Logger.Log(exc.Message, LogType.ERROR);

            }
        }

        private static void SetUserScore(string UserId, Dictionary<int, int> Answers, int Correct, int Incorrect, bool EnableOverride = false)
        {
            using (var db = ApplicationDbContext.Create())
            {
                var Entity = db.Game5_Scores.Find(UserId);
                if (Entity == null)
                {
                    Entity = new Models.Game5_UserScore()
                    {
                        UserId = UserId,
                        Answers = Newtonsoft.Json.JsonConvert.SerializeObject(Answers),
                        CorrectCount = Correct,
                        IncorrectCount = Incorrect
                    };
                    db.Entry<Models.Game5_UserScore>(Entity);
                }
                else if (EnableOverride)
                {
                    Entity.Answers = Newtonsoft.Json.JsonConvert.SerializeObject(Answers);
                    Entity.CorrectCount = Correct;
                    Entity.IncorrectCount = Incorrect;
                }
                else
                {
                    return;
                }

                db.SaveChanges();
            }
        }

        public static Game5_UserScore_Dto GetUserScore(string UserId)
        {
            var res = new Game5_UserScore_Dto();
            using (var db = ApplicationDbContext.Create())
            {
                var ScoreEntity = (from gs in db.Game5_Scores where gs.UserId == UserId select gs).ToList().FirstOrDefault();
                if (ScoreEntity == null)
                {
                    ScoreEntity = new Models.Game5_UserScore()
                    {
                        UserId = UserId,
                        Answers = "",
                        IncorrectCount = 0,
                        CorrectCount = 0
                    };
                }
                var Game = GameHelper.GetGame(GameManager.GameDict[GameKey], db);

                if (Game != null)
                {
                    res = GenerateUserScore(UserId, ScoreEntity.Answers, ScoreEntity.CorrectCount, ScoreEntity.IncorrectCount, Game.Multiplier);
                }
            }

            return res;
        }

        public static List<Game5_UserScore_Dto> GetUsersScore()
        {
            var res = new List<Game5_UserScore_Dto>();
            using (var db = ApplicationDbContext.Create())
            {
                var Game = GameHelper.GetGame(GameManager.GameDict[GameKey], db);
                if (Game != null)
                {
                    res = (from gs in db.Game5_Scores select gs).Select(gs => GenerateUserScore(gs.UserId, gs.Answers, gs.CorrectCount, gs.IncorrectCount, Game.Multiplier)).ToList();
                }
            }

            return res;
        }

        private static Game5_UserScore_Dto GenerateUserScore(string UserId, string Answers, int CorrectCount, int FailCount, double Multiplier)
        {
            var res = new Game5_UserScore_Dto()
            {
                GameId = GameManager.GameDict[GameKey],
                UserId = UserId,
                Answers = Answers,
                Computed_Score = Multiplier * CorrectCount
            };
            return res;
        }

    }
}