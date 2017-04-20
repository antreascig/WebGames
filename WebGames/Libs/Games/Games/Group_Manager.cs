using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebGames.Models;

namespace WebGames.Libs.Games.Games
{
    public class Group_Manager
    {
        //public static int GameId
        //{
        //    get
        //    {
        //        return GameManager.GameDict[GameKeys.GAME_5].GameId;
        //    }
        //}

        //public static bool Groups_Generated = false;

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

        private static void SetGroups(Dictionary<int, List<string>> Groups)
        {
            if (Groups == null) return;

            using (var db = ApplicationDbContext.Create())
            {
                var userScores = new List<User_Group>();
                foreach (var group in Groups)
                {
                    foreach (var userId in group.Value)
                    {
                        userScores.Add(new User_Group()
                        {
                            UserId = userId,
                            GroupNumber = group.Key,
                        });
                    }
                }
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
                    var QuestionsGame = (from game in db.Games where game.GameId == GameManager.GameDict[GameKeys.Questions].GameId select game).SingleOrDefault();
                    // Get the scores 
                    var scores = (from score in db.Questions_Scores select score).ToList();
                    foreach (var user in UserGroups)
                    {
                        if (!res.ContainsKey(user.GroupNumber))
                        {
                            res.Add(user.GroupNumber, new List<UserTotalScore>());
                        }

                        var UserGroupData = scores.FirstOrDefault(u => u.UserId == user.UserId);
                        UserTotalScore UserScore = new UserTotalScore()
                        {
                            UserId = user.UserId,
                            User_FullName = user.User.FullName,
                        };
                        if (UserGroupData != null)
                        {
                            UserScore.Score = ScoreManager.CalculateScore(QuestionsGame, UserGroupData.Tokens);
                        }
                        res[user.GroupNumber].Add(UserScore);
                    }

                    return res;
                }
            }

            var AtomicGames = new string[] { GameKeys.Adespotabalakia, GameKeys.Juggler, GameKeys.Mastermind, GameKeys.Escape_1, GameKeys.Escape_2, GameKeys.Escape_3, GameKeys.Whackamole };
            var UserScores = ScoreManager.GetUsersTotalScoresForGames(AtomicGames);

            var TopUserScores = UserScores.OrderByDescending(s => s.Score).Take(144).ToList();

            for (var i = 0; i < TopUserScores.Count; i++)
            {
                int GroupNumber = (int)( i % 12) + 1;
                if (!res.ContainsKey(GroupNumber))
                {
                    res.Add(GroupNumber, new List<UserTotalScore>());
                }
                res[GroupNumber].Add(TopUserScores[i]);
            }
            return res;
        }
    }
}