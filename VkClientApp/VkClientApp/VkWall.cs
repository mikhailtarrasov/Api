using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VkApiDll;

namespace VkClientApp
{
    class VkWall
    {
        private List<VkPost> _postList;

        public List<VkPost> PostList
        {
            get { return _postList; }
            private set { _postList = value; }
        }

        public VkWall()
        {
            PostList = null;
        }

        private bool GetUserWall(VkUser vkUser)
        {
            int offset = 0;
            int count = 100;

            VkApi ApiObj = new VkApi();

            VkApiResponse<PostDTO> resp = ApiObj.Get100Posts(vkUser.Id.ToString(), offset, count);

            if (resp.Response == null || resp.Response.Count == 0)
            {
                return false;
            }
            else
            {
                PostList = new List<VkPost>(resp.Response.Count);

                for (int i = resp.Response.Count; i > 0; i -= 100)
                {
                    Thread.Sleep(330);
                    if (resp.Response != null)
                    {
                        foreach (PostDTO post in resp.Response.Items)
                        {
                            VkPost vkPost = new VkPost(post);
                            PostList.Add(vkPost);
                        }

                        if (i > 100)
                        {
                            offset += 100;
                            resp = ApiObj.Get100Posts(vkUser.Id.ToString(), offset, count);
                        }
                    }
                }
                return true;
            }
        }

        private bool GetNews(VkUser user)
        {
            if (user.FriendsList != null)
            {
                PostList = new List<VkPost>();

                foreach (VkUser friend in user.FriendsList)
                {
                    VkWall friendWall = new VkWall();

                    if (friendWall.GetUserWall(friend))     // Если стена есть
                    {
                        foreach (VkPost post in friendWall.PostList)
                        {
                            PostList.Add(post);
                        }
                    }
                }
                if (PostList.Count() > 0) return true;      // Если новостная лента НЕ пустая
            }
            return false;                                   // Если новостная лента пустая
        }

        private void SortNews()
        {
            PostList.Sort(Comparer<VkPost>.Create((post1, post2) => post2.LikesCount - post1.LikesCount));
        }

        public bool GetSortedNews(VkUser user)
        {
            if (GetNews(user))                              // Если новостная лента НЕ пустая
            {
                SortNews();
                return true;
            }
            else return false;
        }
    }
}
