using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using VkApiDll;
using System.Windows.Forms;

namespace VkClientApp
{
    class VkClient
    {
        public VkClient()
        {
            /*------------------------- OAuth авторизация -------------------------*/
            //Console.WriteLine("Сейчас откроется вкладка браузера, необходимо скопировать из строки браузера access_token, для продолжения нажмите любую клавишу..");
            //Console.ReadKey();

            //Process.Start("https://oauth.vk.com/authorize?client_id=" + VkApi.ClientId + "&display=popup&redirect_uri=" + VkApi.RedirectUri + "&scope=" + VkApi.Scope + "&response_type=token&v=" + VkApi.Version);

            //Console.WriteLine("Теперь вставляйте access_token: ");
            //VkApi.VkAccessToken = (Console.ReadLine());
            /*---------------------------------------------------------------------*/

            VkApi.VkAccessToken = "e2a492172d5982be8e5f991aa4634783028d56d25d4f4ce708866f3347d977d333249372f6198c15447c1";
        }

        public void OAuth()
        {
            //Webbrowser.Navigate(VkApi.AuthUrl);
        }

        public static List<VkUser> GetGroupMembersGraph(String groupName)
        {
            VkApi ApiObj = new VkApi();

            VkApiResponse<UserDTO> resp = ApiObj.GetGroupsMembers(groupName);

            List<VkUser> groupMembersGraph = new List<VkUser>(resp.Response.Count);
            int i = 0;

            foreach (UserDTO user in resp.Response.Items)
            {
                groupMembersGraph.Insert(i, new VkUser(user));
                groupMembersGraph[i].SetFriends();

                i++;
                Thread.Sleep(220);
            }

            //int [] groupMembersIds = VkApi.GetGroupsMembersIds(groupName);
            //List<VkUser> groupMembersGraph = new List<VkUser>(groupMembersIds.Length);

            //foreach (int id in groupMembersIds)
            //{
            //    VkUser userGraph = GetUserGraphByUsername(id.ToString());
            //    groupMembersGraph.Add(userGraph);
            //    Thread.Sleep(400);
            //}

            return groupMembersGraph;
        }

        public VkUser GetUserGraphByUsername(String username)
        {
            VkApi ApiObj = new VkApi();

            VkUser user = new VkUser(ApiObj.GetUserByUsername(username));
            user.SetFriends();
            return user;
        }

        static void Main(string[] args)
        {
            Stopwatch timeGetMembersFriends = new Stopwatch();  /*  Старт секндомера */
            timeGetMembersFriends.Start();                      /*                   */

            VkApi apiObj = new VkApi();
            VkClient vkClient = new VkClient();

            String groupName = "csu_iit";
            VkApiResponse<UserDTO> groupUsers = apiObj.GetGroupsMembers(groupName);

            int i = 1;
            foreach (UserDTO grUser in groupUsers.Response.Items)
            {
                Console.WriteLine(i++ + ". " + grUser.LastName + " " + grUser.FirstName);
            }

            Console.WriteLine("Введите номер участника группы для получения его новостей: ");
            String strN = Console.ReadLine();

            int numberOfMember = -1;
            VkWall usersSortedNews = null;

            try
            {
                numberOfMember = Int32.Parse(strN);

                if (numberOfMember > 0 && numberOfMember <= groupUsers.Response.Count)
                {
                    usersSortedNews = new VkWall();
                    VkUser currentUser = vkClient.GetUserGraphByUsername(groupUsers.Response.Items[numberOfMember - 1].Id.ToString());
                     
                    usersSortedNews.GetSortedNews(currentUser);
                }
            }
            catch (FormatException e)
            {
                Console.WriteLine(e.Message);
            }

            int lastPostOwnerId = 0;
            UserDTO curr = null;

            if (usersSortedNews != null)
            {
                int k = 0;
                foreach (VkPost post in usersSortedNews.PostList)
                {
                    if (k++ >= 99) break;

                    if (lastPostOwnerId != post.OwnerId)
                    {
                        lastPostOwnerId = post.OwnerId;
                        curr = apiObj.GetUserByUsername(post.OwnerId.ToString());
                        Thread.Sleep(330);
                    }
                    Console.WriteLine(k + ". Владелец стены: " + curr.LastName + " " + curr.FirstName +
                                      " Количество лайков:" + post.LikesCount + " Текст: " + post.Text);
                }
                //List<VkUser> groupMembersList = Class.GetGroupMembersGraph(groupName);  
                //273
                Console.WriteLine("Количество постов в списке: " + usersSortedNews.PostList.Count);
                Console.WriteLine("Вывели первые " + k + 1 + " задач по популярности.");
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
