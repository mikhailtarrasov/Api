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

            VkApi.VkAccessToken = "01e5a4e98bbbb68b54f22e4779fa8d3c05eede5a3b7a61664536c1eab62c1e8e2b557820e3641828da1f1";

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
            List<VkUser> vkGroupsMembersList = null;

            VkApiResponse<UserDTO> response = api.GetGroupsMembers(groupId);

            if (response != null && response.Response.Count > 0)
            {
                vkGroupsMembersList = new List<VkUser>();
                foreach (UserDTO user in response.Response.Items)
                {
                    vkGroupsMembersList.Add(new VkUser(user));
                }
            }
            return vkGroupsMembersList;
        }

        public VkUser GetVkUserByUsername(String username)
        {
            VkApi api = new VkApi();

            return new VkUser(api.GetUserByUsername(username));
        }

        public List<VkUser> GetGroupMembersGraph(String groupName) 
        {
            var groupMembersGraph = GetVkGroupsMembers(groupName);

            for (int i = 0; i < groupMembersGraph.Count; i++)
            {
                groupMembersGraph[i].SetFriends();
                Thread.Sleep(220);
            }

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
