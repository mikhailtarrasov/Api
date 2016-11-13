using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VkClientApp;
using System.Diagnostics;
using System.Threading;

namespace VkDatabaseApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch timeGetMembersFriends = new Stopwatch();  /*  Старт секндомера */
            timeGetMembersFriends.Start();                      /*                   */

            VkClient vkClient = new VkClient();

            String groupName = "csu_iit";
            List<VkUser> listGroupMembers = vkClient.GetVkGroupsMembers(groupName);
            
            int i = 1;
            foreach (VkUser grUser in listGroupMembers)
            {
                Console.WriteLine(i++ + ". " + grUser.LastName + " " + grUser.FirstName);
            }

            Console.WriteLine("Введите номер участника группы для получения его новостей: ");
            String strN = Console.ReadLine();

            VkWall usersSortedNews = null;

            //

            try
            {
                int numberOfMember = -1;
                numberOfMember = Int32.Parse(strN);

                if (numberOfMember > 0 && numberOfMember <= listGroupMembers.Count)
                {
                    usersSortedNews = new VkWall();
                    VkUser currentUser = vkClient.GetUserGraphByUsername(listGroupMembers[numberOfMember - 1].Id.ToString());

                    usersSortedNews.GetSortedNews(currentUser);
                }
            }
            catch (FormatException e)
            {
                Console.WriteLine(e.Message);
            }

            int lastPostOwnerId = 0;
            VkUser curr = null;

            if (usersSortedNews != null)
            {
                int k = 0;
                foreach (VkPost post in usersSortedNews.PostList)
                {
                    if (k++ >= 99) break;

                    if (lastPostOwnerId != post.OwnerId)
                    {
                        lastPostOwnerId = post.OwnerId;
                        curr = vkClient.GetVkUserByUsername(post.OwnerId.ToString());
                        Thread.Sleep(330);
                    }
                    Console.WriteLine(k + ". Владелец стены: " + curr.LastName + " " + curr.FirstName +
                                      " Количество лайков:" + post.LikesCount + " Текст: " + post.Text);
                }
                //List<VkUser> groupMembersList = Class.GetGroupMembersGraph(groupName);  
                //273
                Console.WriteLine("Количество постов в списке: " + usersSortedNews.PostList.Count);
                Console.WriteLine("Вывели первые " + k + " постов по популярности.");
            }
            else Console.WriteLine("Список записей пуст!");



            /*---------------------------------------------------------------------*/
            timeGetMembersFriends.Stop();
            Console.WriteLine("Время работы: " + FormatTime(timeGetMembersFriends));
            /*---------------------------------------------------------------------*/

            Console.ReadKey();
        }

        public static String FormatTime(Stopwatch time)
        {
            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = time.Elapsed;

            // Format and display the TimeSpan value.
            return String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
        }
    }
}
