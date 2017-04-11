using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebGames.Models;

namespace WebGames.Libs.Games.Games
{
    public class Game6_Manager
    {
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

                return GroupNumber;
            }
            catch (Exception exc)
            {
                Logger.Log(exc);
                throw exc;
            }
        }

        public static Dictionary<int, List<UserTotalScore>> GetGroups()
        {
            var Groups = new Dictionary<int, List<UserTotalScore>>(); // <groupNumber, list of UserScoreVM>

            var AtomicGames = new string[] { GameKeys.GAME_1, GameKeys.GAME_2, GameKeys.GAME_3, GameKeys.GAME_4_1, GameKeys.GAME_4_2, GameKeys.GAME_4_3, GameKeys.GAME_5 };
            var UserScores = ScoreManager.GetUsersTotalScoresForGames(AtomicGames);

            var TopUserScores = UserScores.OrderByDescending(s => s.Score).Take(144).ToList();

            for (var i = 0; i < TopUserScores.Count; i++)
            {
                int GroupNumber = (12 / i) + 1;
                if (!Groups.ContainsKey(GroupNumber))
                {
                    Groups.Add(GroupNumber, new List<UserTotalScore>());
                }
                Groups[GroupNumber].Add(TopUserScores[i]);
            }
            return Groups;
        }
    }
}