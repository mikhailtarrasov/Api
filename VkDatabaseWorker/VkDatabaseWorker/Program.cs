using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using VkClientApp;
using VkDatabaseDll;

namespace VkDatabaseWorker
{
    class Program
    {
        static void Main(string[] args)
        {
            string groupName = "programm_exam";
            Stopwatch timeGetGroupMembersFriendsWalls = new Stopwatch();
            Stopwatch FillPostsInDB = new Stopwatch();

            var vkClient = new VkClient();
            List<VkUser> listGroupMembers = vkClient.GetGroupMembersGraph(groupName);

            var efClient = new EFDatabaseClient();
            efClient.CleanDatabase();
            efClient.FillGroupMembersInDatabase(listGroupMembers, groupName);

            timeGetGroupMembersFriendsWalls.Start();
            var userWallDictionary = vkClient.GetWallsForFriendsInUserGraph(listGroupMembers);
            timeGetGroupMembersFriendsWalls.Stop();
            Console.WriteLine("------------------------------------------------------------------\n" +
                              "Время получения постов для друзей членов сообщества: {0}\n" +
                              "------------------------------------------------------------------", 
                              FormatTime(timeGetGroupMembersFriendsWalls));
            
            FillPostsInDB.Start();
            efClient.FillNewsForDbGroupMembers(groupName, userWallDictionary);
            FillPostsInDB.Stop();
            Console.WriteLine("Время записи всех постов для друзей членов сообщества в БД: {0}\n" +
                              "------------------------------------------------------------------", 
                              FormatTime(FillPostsInDB));

            Console.ReadKey();
            Console.ReadKey();
        }

        public static String FormatTime(Stopwatch time)
        {
            return time.Elapsed.ToString("g");
        }
    }
}
