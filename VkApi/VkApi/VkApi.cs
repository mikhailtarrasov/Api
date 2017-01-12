using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using VkApiDll.Serialization;

namespace VkApiDll
{
    public class VkApi
    {
        private const int client_id = 5658746;
        private const string redirect_uri = "https://oauth.vk.com/blank.html";
        private const string scope = "wall,friends,photos";
        private const string clientSecret = "RXPswbMZNISw6pVpp56H";
        private const string version = "5.57";

        public static string VkAccessToken { get; set; }
        public static int ClientId { get { return client_id; } }
        public static string RedirectUri { get { return redirect_uri; } }
        public static string Scope { get { return scope; } }
        public static string ClientSecret { get { return clientSecret; } }
        public static string Version { get { return version; } }


        
        private string SendRequest(string method_name, Dictionary<string, string> parameters)
        {
            WebClient webClient = new WebClient();
            webClient.Encoding = System.Text.Encoding.UTF8;

            foreach (KeyValuePair<string, string> param in parameters)
            {
                webClient.QueryString.Add(param.Key.ToString(), param.Value.ToString());
            }
            webClient.QueryString.Add("v", Version);
            //webClient.QueryString.Add("access_token", VkAccessToken);
            return webClient.DownloadString("https://api.vk.com/method/" + method_name);
        }

        private T ParseResponse<T>(string jsonResponse)
        {
            using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(jsonResponse)))
            {
                var serializer = new DataContractJsonSerializer(typeof(T));
                return (T)serializer.ReadObject(ms);
            }
        }

        public VkApiResponse<UserDTO> GetGroupsMembers(String groupId)
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("group_id", groupId);
            parameters.Add("fields", "lists, photo_50");
            String json = SendRequest("groups.getMembers", parameters);
            return ParseResponse<VkApiResponse<UserDTO>>(json);
        }

        public int[] GetGroupsMembersIds(String groupId)
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("group_id", groupId);
            String json = SendRequest("groups.getMembers", parameters);
            return ParseResponse<VkApiResponse<int>>(json).Response.Items;
        }

        public VkApiResponse<UserDTO> GetFriends(String userId)
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("user_id", userId);
            parameters.Add("fields", "lists, photo_50");
            String json = SendRequest("friends.get", parameters);
            return ParseResponse<VkApiResponse<UserDTO>>(json);
        }

        public UserDTO GetUserByUsername(String username)
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("user_ids", username);
            parameters.Add("fields", "photo_50");
            String json = SendRequest("users.get", parameters);
            return ParseResponse<VkApiResponse>(json).Response[0];
        }

        public VkApiResponse<PostDTO> GetOwnerPostsFromWall(String ownerId, int offset, int count)
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("owner_id", ownerId);
            parameters.Add("offset", offset.ToString());
            parameters.Add("count", count.ToString());
            parameters.Add("filter", "owner");
            String json = SendRequest("wall.get", parameters);
            return ParseResponse<VkApiResponse<PostDTO>>(json);
        }
    }
}