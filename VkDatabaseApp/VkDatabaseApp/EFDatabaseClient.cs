using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using VkClientApp;
using VkDatabaseDll.Domain;
using VkDatabaseDll.Domain.Entity;

namespace VkDatabaseDll
{
    public class EFDatabaseClient
    {
        public Group GetGroupByScreenName(string screenName)
        {
            return new DatabaseContext().Groups.FirstOrDefault(x => x.ScreenName == screenName);
        }

        public User GetUserById(int id)
        {
            return new DatabaseContext().Users.Find(id);
        }

        public void FillGroupMembersInDatabase(List<VkUser> listGroupMembers, string groupName)
        {
            Stopwatch timeFillInDatabase = new Stopwatch();           
            timeFillInDatabase.Start();       

            var dbUserIdsHashSet = new HashSet<int>();
            DatabaseContext db = null;
            int j = 0;
            try
            {
                db = new DatabaseContext();
                db.Configuration.AutoDetectChangesEnabled = false;
                db.Configuration.ValidateOnSaveEnabled = false;
                var dbGroup = db.Groups.FirstOrDefault(x => x.ScreenName == groupName);
                if (dbGroup == null) dbGroup = db.Groups.Add(new Group(groupName));

                foreach (var user in listGroupMembers)
                {
                    var userFriends = user.FriendsList;

                    User dbUser;

                    bool userInDatabase = dbUserIdsHashSet.Contains(user.Id);
                    if (userInDatabase)
                        dbUser = db.Users.Find(user.Id);
                    else
                    {
                        dbUser = new User(user);
                        db.Set<User>().Add(dbUser);
                        dbUserIdsHashSet.Add(user.Id);
                    }
                    dbGroup.MembersList.Add(dbUser);

                    List<User> friendsList = new List<User>();
                    if (userFriends != null && userFriends.Count > 0)
                    {
                        foreach (var friend in userFriends)
                        {
                            User dbFriend;
                            if (dbUserIdsHashSet.Contains(friend.Id))
                                dbFriend = db.Users.Find(friend.Id);
                            else
                            {
                                dbFriend = new User(friend);
                                dbUserIdsHashSet.Add(friend.Id);
                            }
                            friendsList.Add(dbFriend);
                        }
                    }
                    dbUser.Friends.AddRange(friendsList);
                    dbGroup.MembersList.Add(dbUser);

                    ++j;
                    if (listGroupMembers.Count > 100)
                        if ((j%(listGroupMembers.Count/100)) == 0) Console.Write("█");

                    db.ChangeTracker.DetectChanges();
                    if (user.FriendsList != null && user.FriendsList.Count > 100)
                    {
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                db.ChangeTracker.DetectChanges();
                db.SaveChanges(); 
                if (db != null) db.Dispose();
            }
            /*---------------------------------------------------------------------*/
            timeFillInDatabase.Stop();
            Console.WriteLine("------------------------------------------------------------------\n" +
                                "Кол-во юзеров - {1}\tОбщее время записи графа в БД: {0}", FormatTime(timeFillInDatabase), dbUserIdsHashSet.Count);
            /*---------------------------------------------------------------------*/
        }

        public DatabaseContext RecreateContext(DatabaseContext db)    
        {
            try
            {
                db.ChangeTracker.DetectChanges();
                db.SaveChanges();
                db.Dispose();
                db = new DatabaseContext();
                db.Configuration.AutoDetectChangesEnabled = false;
                db.Configuration.ValidateOnSaveEnabled = false;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, e.HelpLink);
            }
            
            return db;
        }

        public void FillNewsForDbGroupMembers(string groupName, Dictionary<int, VkWall> userWallsDictionary)
        {
            using (var db = new DatabaseContext())
            {
                var group = db.Groups.FirstOrDefault(x => x.ScreenName == groupName);
                if (group != null && group.MembersList.Count > 0)
                {
                    foreach (var groupMember in group.MembersList)
                    {
                        if (groupMember.Friends.Count > 0)
                        {
                            foreach (var friend in groupMember.Friends)
                            {
                                if (!userWallsDictionary.ContainsKey(friend.VkId))
                                    continue;

                                var newWall = new VkWall();
                                userWallsDictionary.TryGetValue(friend.VkId, out newWall);
                                foreach (var post in newWall.PostList)
                                {
                                    var user = db.Users.Find(friend.VkId);
                                    user.Posts.Add(new Post(post, newWall.AvgPostReaction));
                                    db.SaveChanges();
                                }
                            }
                        }
                    }
                }
            }
        }

        public void CleanDatabase()
        {
            Stopwatch timeDelete = new Stopwatch(); 
            timeDelete.Start(); 
            using (var db = new DatabaseContext())
            {
                Console.Write("Количество пользователей в базе перед удалением: {0}", db.Users.Count());
                try
                {
                    db.Database.ExecuteSqlCommand("TRUNCATE TABLE UserUsers");
                    db.Database.ExecuteSqlCommand("TRUNCATE TABLE UserGroups");
                    db.Photos.RemoveRange(db.Photos);
                    db.Links.RemoveRange(db.Links);
                    db.PostAttachments.RemoveRange(db.PostAttachments);
                    db.SaveChanges();
                    Console.Write("\t☺");
                    db.Posts.RemoveRange(db.Posts);
                    db.SaveChanges();
                    Console.Write("☻");
                    db.Users.RemoveRange(db.Users);
                    db.SaveChanges();
                    Console.Write("☺\n");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                Console.WriteLine("Количество пользователей в базе после удаления:  {0}\n", db.Users.Count());
                Console.WriteLine("Количество постов, прикреплений, фотографий и ссылок в базе после удаления: {0} {1} {2} {3}", db.Posts.Count(), db.PostAttachments.Count(), db.Photos.Count(), db.Links.Count());
            }
            timeDelete.Stop();
            Console.WriteLine("Время на удаление: " + FormatTime(timeDelete));
        }

        public static String FormatTime(Stopwatch time)
        {
            return time.Elapsed.ToString("g");
        }
    }
}
// 230 строк