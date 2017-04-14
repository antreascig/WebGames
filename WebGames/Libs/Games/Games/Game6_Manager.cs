using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebGames.Models;

namespace WebGames.Libs.Games.Games
{
    public class Game6_Manager
    {
        public static int GameId
        {
            get
            {
                return GameManager.GameDict[GameKeys.GAME_5].GameId;
            }
        }

        public static bool Groups_Generated = false;

        public static int GetUserGroup(string UserId)
        {
            try
            {
                var GroupNumber = -1;
                Dictionary<int, List<UserTotalScore>> Groups = null;

                if (Groups_Generated)
                {
                    using (var db = ApplicationDbContext.Create())
                    {
                        var User_Group = (from ug in db.Game6_User_Groups where ug.UserId == UserId select ug).SingleOrDefault();
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
                else
                {
                    Groups = GetGroups();
                    var Group = Groups.Where(g => g.Value.FirstOrDefault(u => u.UserId == UserId) != null);
                    if (Group.Any())
                    {
                        GroupNumber = Group.FirstOrDefault().Key;
                    }

                    SetGroups(Groups);
                }

                return GroupNumber;
            }
            catch (Exception exc)
            {
                Logger.Log(exc);
                throw exc;
            }
        }

        private static void SetGroups(Dictionary<int, List<UserTotalScore>> Groups)
        {
            //Should Run Only Once
            Task.Run(() =>
            {
                if (Groups == null) return;

                using (var db = ApplicationDbContext.Create())
                {
                    var userScores = new List<Game6_User_Group>();
                    foreach (var group in Groups)
                    {
                        foreach (var user in group.Value)
                        {
                            userScores.Add(new Game6_User_Group()
                            {
                                UserId = user.UserId,
                                GroupNumber = group.Key,
                            });
                        }
                    }
                    db.Game6_User_Groups.AddRange(userScores);
                    db.SaveChangesAsync();
                }
            });
        }

        public static Dictionary<int, List<UserTotalScore>> GetGroups()
        {
            var res = new Dictionary<int, List<UserTotalScore>>(); // <groupNumber, list of UserScoreVM>

            using (var db = ApplicationDbContext.Create())
            {
                var UserGroups = (from ug in db.Game6_User_Groups select ug).Include("User").ToList();
                if (UserGroups.Any())
                {
                    var Game6 = (from game in db.Games where game.GameId == GameId select game).SingleOrDefault();
                    // Get the scores 
                    var scores = (from score in db.Game6_Scores select score).ToList();
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
                            UserScore.Score = ScoreManager.CalculateScore(Game6, UserGroupData.Tokens);
                        }
                        res[user.GroupNumber].Add(UserScore);
                    }

                    return res;
                }
            }

            var AtomicGames = new string[] { GameKeys.GAME_1, GameKeys.GAME_2, GameKeys.GAME_3, GameKeys.GAME_4_1, GameKeys.GAME_4_2, GameKeys.GAME_4_3, GameKeys.GAME_5 };
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