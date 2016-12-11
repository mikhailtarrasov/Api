using System;
using System.Collections.Generic;
using VkApiDll;

namespace VkClientApp
{
    public class VkUser
    {
        public int Id { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Photo50 { get; private set; }
        public List<VkUser> FriendsList { get; private set; }

        public VkUser(UserDTO user)
        {
            Id = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Photo50 = user.Photo50;
            FriendsList = null;
        }

        public void AddFriend(UserDTO friend)
        {
            VkUser newFriend = new VkUser(friend);
            FriendsList.Add(newFriend);
        }

        public void SetFriends()
        {
            VkApi vkApi = new VkApi();

            VkApiResponse<UserDTO> friendsResp = vkApi.GetFriends(Id.ToString());

            if (friendsResp.Response != null)
            {
                FriendsList = new List<VkUser>();

                foreach (UserDTO friend in friendsResp.Response.Items)
                    AddFriend(friend);
            }
        }
    }
}
