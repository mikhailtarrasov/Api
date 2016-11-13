using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Forms;
using VkApiDll;

namespace VkClientApp
{
    public class VkClient
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

            VkApi.VkAccessToken = "18de562f410e666c474bd44c5f2b0943535e14869401f08fb41650b17d1b1d209ee3ef4d501ddf0b32bed";

            //OAuth();
        }

        //public void OAuth()
        //{
        //    string authUrl = "https://oauth.vk.com/authorize?client_id=" + VkApi.ClientId + "&display=popup&redirect_uri=" + VkApi.RedirectUri +
        //                     "&scope=" + VkApi.Scope + "&response_type=token&v=" + VkApi.Version;

        //    WebBrowser browser = new WebBrowser();
        //    browser.Navigate(authUrl);
        //}
        public List<VkUser> GetVkGroupsMembers(String groupId)
        {
            VkApi api = new VkApi();
            List<VkUser> listVkUsers = null;

            VkApiResponse<UserDTO> response = api.GetGroupsMembers(groupId);

            if (response != null)
            {
                if (response.Response.Count > 0)
                {
                    listVkUsers = new List<VkUser>();
                    foreach (UserDTO user in response.Response.Items)
                    {
                        listVkUsers.Add(new VkUser(user));
                    }
                }
            }
            return listVkUsers;
        }

        public VkUser GetVkUserByUsername(String username)
        {
            VkApi api = new VkApi();

            return new VkUser(api.GetUserByUsername(username));
        }

        public List<VkUser> GetGroupMembersGraph(String groupName)
        {
            VkApi api = new VkApi();

            VkApiResponse<UserDTO> resp = api.GetGroupsMembers(groupName);

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
    }
}
