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
            Console.WriteLine("Сейчас откроется вкладка браузера, необходимо скопировать из строки браузера access_token, для продолжения нажмите любую клавишу..");
            Console.ReadKey();

            Process.Start("https://oauth.vk.com/authorize?client_id=" + VkApi.ClientId + "&display=popup&redirect_uri=" + VkApi.RedirectUri + "&scope=" + VkApi.Scope + "&response_type=token&v=" + VkApi.Version);

            Console.WriteLine("Теперь вставляйте access_token: ");
            VkApi.VkAccessToken = (Console.ReadLine());
            /*---------------------------------------------------------------------*/

            //VkApi.SetVkAccessToken("6bcd98b589fcbebc5590ed47df4ca77d61ed1dba6ebf33fcaa5ee277296c882c39c51e824c9a84a34fc27");
        }

        public void OAuth()
        {
            //Webbrowser.Navigate(VkApi.AuthUrl);
        }

        public List<PostDTO> GetNews(VkUser user)
        {
            List<PostDTO> newsList = null;
            if (user.FriendsList != null)
            {
                newsList = new List<PostDTO>();

                foreach (VkUser friend in user.FriendsList)
                {
                    List<PostDTO> friendWall = GetWall(friend.Id.ToString());
                    if (friendWall != null)
                    {
                        foreach (PostDTO post in friendWall)
                        {
                            newsList.Add(post);
                        }
                    }
                }
                return newsList;
            }
            return newsList;
        }

        public List<PostDTO> SortNews(List<PostDTO> newsList)
        {
            newsList.Sort(Comparer<PostDTO>.Create((post1, post2) => post2.Likes.Count - post1.Likes.Count));
            return newsList;
        }

        public List<PostDTO> GetSortedNews(VkUser user)
        {
            return SortNews(GetNews(user));
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

        public List<PostDTO> GetWall(String ownerId)
        {
            int offset = 0;
            int count = 100;

            VkApi ApiObj = new VkApi(); 

            List<PostDTO> postsList = null;

            VkApiResponse<PostDTO> resp = ApiObj.Get100Posts(ownerId, offset, count);

            if (resp.Response == null || resp.Response.Count == 0)
            {
                return postsList;
            }
            else
            {
                postsList = new List<PostDTO>(resp.Response.Count);

                int i = resp.Response.Count;
                while (i > 0)
                {
                    Thread.Sleep(330);
                    if (resp.Response != null)
                    {
                        foreach (PostDTO post in resp.Response.Items)
                        {
                            postsList.Add(post);
                        }

                        if (i > 100)
                        {
                            offset += 100;
                            resp = ApiObj.Get100Posts(ownerId, offset, count);
                        }
                        i -= 100;
                    }
                }

                return postsList;
            }
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
            List<PostDTO> usersSortedNews = null;

            try
            {
                numberOfMember = Int32.Parse(strN);

                if (numberOfMember > 0 && numberOfMember <= groupUsers.Response.Count)
                {
                    VkUser currentUser = vkClient.GetUserGraphByUsername(groupUsers.Response.Items[numberOfMember - 1].Id.ToString());

                    usersSortedNews = vkClient.GetSortedNews(currentUser);
                }
            }
            catch (FormatException e)
            {
                Console.WriteLine(e.Message);
            }

            int lastPostOwnerId = 0;
            UserDTO curr = null;

            int k = 0;
            foreach (PostDTO post in usersSortedNews)
            {
                if (k++ >= 99) break;

                if (lastPostOwnerId != post.OwnerId)
                {
                    lastPostOwnerId = post.OwnerId;
                    curr = apiObj.GetUserByUsername(post.OwnerId.ToString());
                    Thread.Sleep(330);
                }
                Console.WriteLine("Владелец стены: " + curr.LastName + " " + curr.FirstName +
                                  " Количество лайков:" + post.Likes.Count + " Текст: " + post.Text);
            }
            //List<VkUser> groupMembersList = Class.GetGroupMembersGraph(groupName);  
            //273
            Console.WriteLine("Количество постов в списке: " + usersSortedNews.Count);
            Console.WriteLine("Вывели первые " + k + 1 + " задач по популярности.");


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
