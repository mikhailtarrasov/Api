using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VkClientApp;
using VkDatabaseApp.Domain;
using VkDatabaseApp.Domain.Entity;

namespace VkDatabaseApp
{
    public class EFDatabaseClient
    {
        public void FillInDatabase(string groupName)
        {
            CleanUsersTable();

            Stopwatch timeGetMembersFriendsFromVk = new Stopwatch();    
            timeGetMembersFriendsFromVk.Start();

            var vkClient = new VkClient();
            Console.WriteLine("-------------------------------------\nПолучение графа из вк...");
            List<VkUser> listGroupMembers = vkClient.GetGroupMembersGraph(groupName);

            //var vkClient = new VkClient();
            //List<VkUser> listGroupMembers = new List<VkUser>()
            //{
            //    vkClient.GetUserGraphByUsername("mikhailtarrasov"),
            //    vkClient.GetUserGraphByUsername("anna_li_12"),
            //    vkClient.GetUserGraphByUsername("elenka_kolenka")
            //};

            timeGetMembersFriendsFromVk.Stop();
            Console.WriteLine("Время получения графа из вк: {0}\n-------------------------------------", Program.FormatTime(timeGetMembersFriendsFromVk));
            
            Console.WriteLine("Строим граф в локальном контексте БД...");

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
                    bool detectChanges = false;

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
                    //dbGroup.MembersList.Add(dbUser);

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
                            if (!detectChanges && (dbUser == null || dbFriend == null))
                            {
                                ++countDetectionChanges;
                                db.ChangeTracker.DetectChanges();
                                detectChanges = true;
                                if (dbUser == null) dbUser = db.Users.Find(user.Id);
                                if (dbFriend == null) dbFriend = db.Users.Find(friend.Id);
                            }
                            dbUser.Friends.Add(dbFriend);
                        }
                    }
                    dbGroup.MembersList.Add(dbUser);

                    ++j;
                    if (listGroupMembers.Count > 100)
                        if ((j%(listGroupMembers.Count/100)) == 0) Console.Write("█");
                    
                    if (user.FriendsList != null && user.FriendsList.Count > 100)
                    {
                        ++countDetectionChanges;
                        db.ChangeTracker.DetectChanges();
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
                Console.WriteLine("Время записи контекста в БД: " + Program.FormatTime(DB));

                if (db != null) db.Dispose();
            }
            /*---------------------------------------------------------------------*/
            timeFillInDatabase.Stop();
            Console.WriteLine("------------------------------------------------------------------\n" +
                                "Кол-во юзеров - {1}\tОбщее время записи графа в БД: {0}", Program.FormatTime(timeFillInDatabase), dbUserIdsHashSet.Count);
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

        public void CleanUsersTable()
        {
            Stopwatch timeFillMemberWithFriendsInDatabase = new Stopwatch(); 
            timeFillMemberWithFriendsInDatabase.Start(); 

            using (var dbContext = new DatabaseContext())
            {
                Console.Write("Количество пользователей в базе перед удалением: {0}", dbContext.Users.Count());
                try
                {
                    dbContext.Database.ExecuteSqlCommand("TRUNCATE TABLE UserUsers");
                    Console.Write("\t☺☻\n");
                }
                catch (Exception e)
                {
                    //Console.WriteLine(e.Message);
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
            Console.WriteLine("Время на удаление: " + Program.FormatTime(timeFillMemberWithFriendsInDatabase));
        }

        //public void CleanUsersTable2()
        //{
        //    using (var db = new DatabaseContext())
        //    {
        //        int i = 0;
        //        while (true)
        //        {
        //            var user = db.Users.Include(x => x.Friends).FirstOrDefault();
        //            if (user == null) break;
        //            db.Users.Remove(user);
        //            //db.Users.RemoveRange(dbContext.Users);
        //            ++i;
        //            if (i % 100 == 0) db.SaveChanges();
        //        }
        //        db.SaveChanges();
        //    }
        //}

    }
}
