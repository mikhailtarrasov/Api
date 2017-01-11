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
            int countDetectionChanges = 0;
            DatabaseContext db = null;
            int j = 0;
            try
            {
                db = new DatabaseContext();
                db.Configuration.AutoDetectChangesEnabled = false;
                db.Configuration.ValidateOnSaveEnabled = false;
                var dbGroup = db.Groups.FirstOrDefault(x => x.ScreenName == groupName);
                if (dbGroup == null) dbGroup = db.Groups.Add(new Group(groupName));
                else CleanUsersTable();

                foreach (var user in listGroupMembers)
                {
                    Stopwatch timeFillMemberWithFriendsInDatabase = new Stopwatch();
                    timeFillMemberWithFriendsInDatabase.Start();

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

                    ++countDetectionChanges;
                    db.ChangeTracker.DetectChanges();

                    if (user.FriendsList != null && user.FriendsList.Count > 100)
                    {
                        db.SaveChanges();
                    }
                    if (dbUserIdsHashSet.Count > 50000) break;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                ++countDetectionChanges;
                db.ChangeTracker.DetectChanges();
                Console.WriteLine("\n{0} раз обнаруживали изменения для {1} проходов", countDetectionChanges, j);
                Stopwatch DB = new Stopwatch(); /*  Старт секндомера */
                DB.Start(); /*-------------------*/
                Console.WriteLine("Запись контекста непосредственно в саму БД...");
                db.SaveChanges(); 
                DB.Stop();
                Console.WriteLine("Время записи контекста в БД: " + FormatTime(DB));

                if (db != null) db.Dispose();
            }
            /*---------------------------------------------------------------------*/
            timeFillInDatabase.Stop();
            Console.WriteLine("------------------------------------------------------------------\n" +
                                "Кол-во юзеров - {1}\tОбщее время записи графа в БД: {0}", FormatTime(timeFillInDatabase), dbUserIdsHashSet.Count);
            /*---------------------------------------------------------------------*/

            //return dbUserIdsHashSet;
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
            int postsWhenFewTimesTryToAddInDb = 0;
            int photosWhenFewTimesTryToAddInDb = 0;
            using (var db = new DatabaseContext())
            {
                //db.Configuration.AutoDetectChangesEnabled = false;
                //db.Configuration.ValidateOnSaveEnabled = false;

                var postsIdsHashSet = new HashSet<int>();
                var photosIdsHashSet = new HashSet<int>();

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
                                //var vkPostList = userWallsDictionary.FirstOrDefault(x => x.Key.Id == friend.VkId).Value.PostList;
                                foreach (var post in newWall.PostList)
                                {
                                    if (postsIdsHashSet.Contains(post.Id))
                                    {
                                        ++postsWhenFewTimesTryToAddInDb;
                                        continue;
                                    }
                                    postsIdsHashSet.Add(post.Id);
                                    if (post.Attachments != null)
                                        foreach (var attachment in post.Attachments)
                                        {
                                            if (attachment.Type == "photo")
                                            {
                                                if (photosIdsHashSet.Contains(attachment.Photo.Id))
                                                {
                                                    ++photosWhenFewTimesTryToAddInDb;
                                                    continue;
                                                }
                                                photosIdsHashSet.Add(attachment.Photo.Id);
                                            }

                                        }
                                    var user = db.Users.Find(friend.VkId);
                                    user.Posts.Add(new Post(post, newWall.AvgPostReaction));
                                    db.SaveChanges();
                                    //db.Posts.Add(new Post(post, friend, newWall.AvgPostReaction));
                                }
                            }
                        }
                    }
                }
                Console.WriteLine(postsWhenFewTimesTryToAddInDb + " - раз попадался уже существующий пост.");
                Console.WriteLine(photosWhenFewTimesTryToAddInDb + " - раз попадалась уже существующая фотография.");
            }
        }

        public void CleanUsersTable()
        {
            CleanAllPosts();
            Stopwatch timeFillMemberWithFriendsInDatabase = new Stopwatch(); 
            timeFillMemberWithFriendsInDatabase.Start(); 
            using (var dbContext = new DatabaseContext())
            {
                Console.Write("Количество пользователей в базе перед удалением: {0}", dbContext.Users.Count());
                try
                {
                    dbContext.Database.ExecuteSqlCommand("TRUNCATE TABLE UserUsers");
                    dbContext.SaveChanges();
                    Console.Write("\t☺☻\n");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.Write("\t☻☺\n");
                }

                try
                {
                    dbContext.Database.ExecuteSqlCommand("TRUNCATE TABLE UserGroups");
                    dbContext.SaveChanges();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.Write("\t☻☺\n");
                }

                try
                {
                    dbContext.Users.RemoveRange(dbContext.Users);
                    dbContext.SaveChanges();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
                try
                {
                    dbContext.Groups.RemoveRange(dbContext.Groups);
                    dbContext.SaveChanges();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
                Console.WriteLine("Количество пользователей в базе после удаления:  {0}\n", dbContext.Users.Count());
            }
            timeFillMemberWithFriendsInDatabase.Stop();
            Console.WriteLine("Время на удаление: " + FormatTime(timeFillMemberWithFriendsInDatabase));
        }

        public void CleanAllPosts()
        {
            using (var db = new DatabaseContext())
            {
                try
                {
                    db.PostAttachments.RemoveRange(db.PostAttachments);
                    db.Posts.RemoveRange(db.Posts);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }

        public static String FormatTime(Stopwatch time)
        {
            return time.Elapsed.ToString("g");
        }
    }
}
// 230 строк