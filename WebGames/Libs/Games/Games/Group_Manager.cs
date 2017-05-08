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
        public double Score { get; set; }
        public string Controls { get; set; }
    }

    public class GroupScore
    {
        public int Group { get; set; }
        public double TotalScore
        {
            get
            {
                return (UsersInGroup ?? new List<UserTotalScore>()).Sum(u => u.Score);
            }
        }
        public List<UserTotalScore> UsersInGroup { get; set; }
    }

    public class Group_Manager
    {
        public static string[] GroupGames = new string[] { GameKeys.Whackamole, GameKeys.Mastermind };

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


        public static GroupScore GetUserGroupScore(string UserId)
        {
            try
            {

                using (var db = ApplicationDbContext.Create())
                {
                    var User_Group = (from ug in db.User_Groups where ug.UserId == UserId select ug).SingleOrDefault();
                    if (User_Group == null) return null;

                    var Group = User_Group.GroupNumber;

                    var Group_Users = (from ug in db.User_Groups where ug.GroupNumber == Group select ug).ToList();

                    var user_scores = ScoreManager.GetUsersTotalScoresForGames(GameManager.GameDict.Keys.ToArray(), Group_Users.Select(u => u.UserId).ToArray());
                    var res = new GroupScore() { Group = Group, UsersInGroup = user_scores };
                    return res;
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
            if (userScores == null ) return;

            var USDict = userScores.ToDictionary(u => u.UserId);

            using (var db = ApplicationDbContext.Create())
            {
                var Users = db.Users.ToList().ToDictionary(u => u.Id);

                var ToDelete = new List<User_Group>();
                var ToAdd = new List<User_Group>();

                var Existing = db.User_Groups.ToList();
                var ExistingDict = Existing.ToDictionary(u => u.UserId);

                foreach ( var ex in  Existing)
                {
                    if (USDict.ContainsKey(ex.UserId))
                    {
                        if (USDict[ex.UserId].GroupNumber != ex.GroupNumber)
                        {
                            ex.GroupNumber = USDict[ex.UserId].GroupNumber;
                            ex.User_FullName = Users[ex.UserId].FullName;
                        }
                    }
                    else
                    {
                        ToDelete.Add(ex);
                    }
                }

                foreach (var us in userScores)
                {
                    if (!ExistingDict.ContainsKey(us.UserId))
                    {
                        us.User_FullName = Users[us.UserId].FullName;
                        ToAdd.Add(us);
                    }
                }

                // Remove all the entries before 
                if (ToDelete.Any())
                {
                    db.User_Groups.RemoveRange(ToDelete);
                }

                if (ToAdd.Any())
                {
                    db.User_Groups.AddRange(userScores);
                }

                db.SaveChanges();
            }
        }

        public static Dictionary<int, List<UserTotalScore>> GetGroupScores()
        {
            var res = new Dictionary<int, List<UserTotalScore>>(); // <groupNumber, list of UserScoreVM>

            using (var db = ApplicationDbContext.Create())
            {
                var UserGroups = (from ug in db.User_Groups select ug).ToList();

                var UserTotalScoresDict = ScoreManager.GetUsersTotalScoresForGames(GameManager.GameDict.Keys.ToArray()).ToDictionary(u => u.UserId);

                if (UserGroups.Any())
                {
                    foreach (var user in UserGroups)
                    {
                        if (!res.ContainsKey(user.GroupNumber))
                        {
                            res.Add(user.GroupNumber, new List<UserTotalScore>());
                        }

                        UserTotalScore UserScore = UserTotalScoresDict[user.UserId];

                        res[user.GroupNumber].Add(UserScore);
                    }
                }
            }
            return res;
        }

        public static List<UserGroupVM> GetRankingsBeforeGroups()
        {
            var res = new List<UserGroupVM>();

            var AtomicGames = GameManager.GameDict.Keys.Where(g => !GroupGames.Contains(g)).ToArray();// new string[] { GameKeys.Adespotabalakia, GameKeys.Juggler, GameKeys.Mastermind, GameKeys.Escape_1, GameKeys.Escape_2, GameKeys.Escape_3 };
            var UserScores = ScoreManager.GetUsersTotalScoresForGames(AtomicGames);

            var TopUserScores = UserScores.OrderByDescending(s => s.Score).ToList();

            var groupFix = 0;
            for (var i = 0; i < TopUserScores.Count; i++)
            {             
                res.Add(new UserGroupVM()
                {
                    Rank = i + 1,
                    UserId = TopUserScores[i].UserId,
                    User_FullName = TopUserScores[i].User_FullName,
                    Group = (int)(i % 12) + 1 + groupFix * 12,
                    Score = TopUserScores[i].Score
                });

                if ((i + 1) % 144 == 0)
                {
                    groupFix++;
                }
            }
            return res;
        }
    }
}