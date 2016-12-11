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

            //VkApi.VkAccessToken = "8f381c9005cfa5dc7b93e85de92f0ea8965ac1fc721ec3877dd8ed5dc8e8a83fced5f327ddad56ff8b391";
        }

        public List<VkUser> GetVkGroupsMembers(String groupId)
        {
            VkApi api = new VkApi();
            List<VkUser> vkGroupsMembersList = null;

            VkApiResponse<UserDTO> response = api.GetGroupsMembers(groupId);

            if (response != null && response.Response.Count > 0)
            {
                vkGroupsMembersList = new List<VkUser>();
                foreach (UserDTO user in response.Response.Items)
                    vkGroupsMembersList.Add(new VkUser(user));
            }
            return vkGroupsMembersList;
        }

        public VkUser GetVkUserByUsername(String username)
        {
            return new VkUser(new VkApi().GetUserByUsername(username));
        }

        public List<VkUser> GetGroupMembersGraph(String groupName) 
        {
            var groupMembersGraph = GetVkGroupsMembers(groupName);

            for (int i = 0; i < groupMembersGraph.Count; i++)
            {
                groupMembersGraph[i].SetFriends();
                //Thread.Sleep(220);
            }

            return groupMembersGraph;
        }

        public VkUser GetUserGraphByUsername(String username)
        {
            VkUser user = new VkUser(new VkApi().GetUserByUsername(username));
            user.SetFriends();
            return user;
        }
    }
}
