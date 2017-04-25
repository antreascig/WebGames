using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using WebGames.Models;

namespace WebGames.Libs.Games.Games
{
    public class UserGroupVM
    {
        public int Rank { get; set; }
        public string UserId { get; set; }
        public string User_FullName { get; set; }
        public int Group { get; set; }
        public string Controls { get; set; }
    }

    public class Group_Manager
    {
        public static int GetUserGroup(string UserId)
        {
            try
            {

                using (var db = ApplicationDbContext.Create())
                {
                    var User_Group = (from ug in db.User_Groups where ug.UserId == UserId select ug).SingleOrDefault();
                    if (User_Group != null)
                    {
                        return User_Group.GroupNumber;
                    }
                    else
                    {
                        return -1;
                    }
                }
            }
            catch (Exception exc)
            {
                Logger.Log(exc);
                throw exc;
            }
        }

        public static void SetGroups(List<User_Group> userScores)
        {
            if (userScores == null || !userScores.Any()) return;

            using (var db = ApplicationDbContext.Create())
            {
                // Remove all the entries before 
                db.User_Groups.RemoveRange(db.User_Groups);
                db.SaveChanges();
                Thread.Sleep(500);
                db.User_Groups.AddRange(userScores);
                db.SaveChanges();
            }
        }

        public static Dictionary<int, List<UserTotalScore>> GetGroups()
        {
            var res = new Dictionary<int, List<UserTotalScore>>(); // <groupNumber, list of UserScoreVM>

            using (var db = ApplicationDbContext.Create())
            {
                var UserGroups = (from ug in db.User_Groups select ug).Include("User").ToList();
                if (UserGroups.Any())
                {
                    var QuestionGameId = GameManager.GameDict[GameKeys.Questions].GameId;
                    var WhackAMoleGameId = GameManager.GameDict[GameKeys.Whackamole].GameId;
                    var GroupGames = (from game in db.Games where game.GameId == QuestionGameId || game.GameId == WhackAMoleGameId select game).ToList();

                    var QuestionsGame = GroupGames.FirstOrDefault(g => g.GameKey == GameKeys.Questions);
                    var WhackAMoleGame = GroupGames.FirstOrDefault(g => g.GameKey == GameKeys.Whackamole);
                    // Get the scores 
                    var QuestionScores = (from score in db.Questions_Scores select score).ToList();
                    var WhackAMoleScores = (from score in db.Whackamole_Scores select score).ToList();

                    foreach (var user in UserGroups)
                    {
                        if (!res.ContainsKey(user.GroupNumber))
                        {
                            res.Add(user.GroupNumber, new List<UserTotalScore>());
                        }

                        double totalScore = 0;

                        var UserQuestionScoreData = QuestionScores.FirstOrDefault(u => u.UserId == user.UserId);
                        if (UserQuestionScoreData != null)
                        {
                            totalScore += ScoreManager.CalculateScore(QuestionsGame, UserQuestionScoreData.Tokens);
                        }
                        var UserWhackaMoleScoreData = WhackAMoleScores.FirstOrDefault(u => u.UserId == user.UserId);
                        if (UserWhackaMoleScoreData != null)
                        {
                            totalScore += ScoreManager.CalculateScore(WhackAMoleGame, UserWhackaMoleScoreData.Tokens);
                        }

                        UserTotalScore UserScore = new UserTotalScore()
                        {
                            UserId = user.UserId,
                            User_FullName = user.User.FullName,
                            Score = totalScore
                        };

                        res[user.GroupNumber].Add(UserScore);
                    }
                }
            }
            return res;
        }

        public static List<UserGroupVM> GetRankingsBeforeGroups()
        {
            var res = new List<UserGroupVM>();

            var AtomicGames = new string[] { GameKeys.Adespotabalakia, GameKeys.Juggler, GameKeys.Mastermind, GameKeys.Escape_1, GameKeys.Escape_2, GameKeys.Escape_3 };
            var UserScores = ScoreManager.GetUsersTotalScoresForGames(AtomicGames);

            var TopUserScores = UserScores.OrderByDescending(s => s.Score).ToList();

            for (var i = 0; i < TopUserScores.Count; i++)
            {
                    res.Add(new UserGroupVM()
                    {
                        Rank = i + 1,
                        UserId = TopUserScores[i].UserId,
                        User_FullName = TopUserScores[i].User_FullName,
                        Group = (int)(i % 12) + 1
                });
            }
            return res;
        }
    }
}