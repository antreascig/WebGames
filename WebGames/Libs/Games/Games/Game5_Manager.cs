using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebGames.Models;

namespace WebGames.Libs.Games.GameTypes
{
    public class GameQuestionView
    {
        public int QuestionId { get; set; }
        public int QuestionText { get; set; }
        public List<string> Options { get; set; }
    }

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
        public int PointsPerQustion = 100;

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
                return GameManager.GameDict[GameKey].GameId;
            }
        }

        public static List<GameQuestionView> GetPlayerQuestions( string UserId )
        {
            var res = new List<GameQuestionView>();
            var temp = new List<GameQuestionModel>();
            var Ids = new List<int>();
            try
            {
                using (var db = ApplicationDbContext.Create())
                {
                    // Check if questions have already been assigned to the user
                    var UserQuestionsModel = (from uq in db.Game5_User_Questions where uq.UserId == UserId select uq).SingleOrDefault();
                    if (UserQuestionsModel != null && (UserQuestionsModel.Questions ?? "") != "")
                    {
                        var answered = (UserQuestionsModel.Answered ?? "").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        Ids.AddRange(UserQuestionsModel.Questions.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries )
                                                                 // remove any already answered questions
                                                                 .Where(id => !answered.Contains(id))
                                                                 .Select(id => int.Parse(id)) );
                    }
                    // Retrieve the game's metadata
                    var GameMetadata = (Game5_MetaData)GameHelper.GetGameMetaData(GameId, typeof(Game5_MetaData));

                    if (GameMetadata != null && GameMetadata.Questions != null)
                    {
                        // get the score of the user in order to calculate the required number of questions to retrieve
                        var scores = ScoreManager.GetUserTotalScores(UserId);
                        var GamesRequired = new string[] { GameKeys.GAME_1, GameKeys.GAME_2, GameKeys.GAME_3, GameKeys.GAME_4_1, GameKeys.GAME_4_2, GameKeys.GAME_4_3 };
                        var totalScore = scores.Where(s=> GamesRequired.Contains(s.Key) ).Sum(s => s.Value);
                        int NumberOfQuestions = (int) Math.Ceiling( totalScore / GameMetadata.PointsPerQustion );

                        // if not any ids stored in db them choose randomly
                        if ( !Ids.Any() )
                        {
                            var AllQIds = GameMetadata.Questions.Keys.ToList();
                            var RandomNumGen = new Random(DateTime.UtcNow.Millisecond);
                            while (Ids.Count < NumberOfQuestions && NumberOfQuestions < AllQIds.Count )
                            {
                                var randIndex = RandomNumGen.Next(0, AllQIds.Count);
                                var idToAdd = AllQIds[randIndex];
                                if ( !Ids.Contains(idToAdd) )
                                {
                                    Ids.Add(idToAdd);
                                }
                            }

                            db.Game5_User_Questions.Add(new UserQuestion()
                            {
                                UserId = UserId,
                                Questions = string.Join(",", Ids)
                            });
                            db.SaveChanges();
                        }
                    }
                    res.AddRange(
                          GameMetadata.Questions.Where(q => q.Value.Active && Ids.Contains(q.Key))
                                                .Select(q => new GameQuestionView()
                                                {
                                                    QuestionId = q.Value.QuestionId,
                                                    QuestionText = q.Value.QuestionText,
                                                    Options = q.Value.Options,
                                                })
                    );
                }                
            }
            catch (Exception exc)
            {
                Logger.Log(exc.Message, LogType.ERROR);
            }
            return res;
        }

        //public static int Score_Limit { get; set; }
        public static List<GameQuestionModel> GetQuestions()
        {
            var res = new List<GameQuestionModel>();
            try
            {
                var GameMetadata = (Game5_MetaData)GameHelper.GetGameMetaData(GameId, typeof(Game5_MetaData));
                if (GameMetadata != null && GameMetadata.Questions != null)
                {
                    res.AddRange(GameMetadata.Questions.Select(q => q.Value));
                }
            }
            catch(Exception exc)
            {
                Logger.Log(exc.Message, LogType.ERROR);
            }
            return res;
        }

        public static void SaveQuestions(List<GameQuestionModel> Questions)
        {
            using (var db = ApplicationDbContext.Create())
            {
                var Game = GameHelper.GetGame(GameId, db);

                var md = new Game5_MetaData()
                {
                    Questions = Questions.ToDictionary(k => k.QuestionId)
                };
                Game.MetadataJSON = Newtonsoft.Json.JsonConvert.SerializeObject(md);
            }
        }

        public static bool CheckAndSaveQuestionAnswer(string UserId, int QuestionId, int AnswerIndex)
        {
            try
            {
                var isCorrect = false;
                using (var db = ApplicationDbContext.Create())
                {
                    var GameMetadata = (Game5_MetaData)GameHelper.GetGameMetaData(GameId, typeof(Game5_MetaData), db);
                    // Check that is the correct question
                    if (GameMetadata == null
                        || GameMetadata.Questions == null
                        || !GameMetadata.Questions.ContainsKey(QuestionId)
                        || GameMetadata.Questions[QuestionId].QuestionId != QuestionId
                        || GameMetadata.Questions[QuestionId].AnswerIndex != AnswerIndex)
                    {
                        isCorrect = false;
                    }
                    else
                    {
                        isCorrect = true;
                    }

                    // Save question is as answered
                    var User_Questions = (from q in db.Game5_User_Questions where q.UserId == UserId select q).SingleOrDefault();
                    var Existing = (User_Questions.Answered ?? "").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    Existing.Add(QuestionId.ToString());
                    User_Questions.Answered = string.Join(",", Existing);

                    db.SaveChanges();
                }
                
                return isCorrect;
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
                GameManager.GameDict[GameKey].SM.SetUserScore(UserId, Answers.Count, EnableOverride);
            }
            catch (Exception exc)
            {
                Logger.Log(exc.Message, LogType.ERROR);

            }
        }      
    }
}