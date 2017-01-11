using System.Collections.Generic;
using System.Linq;
using VkApiDll;
using VkApiDll.Serialization;

namespace VkClientApp
{
    public class VkWall
    {
        public List<VkPost> PostList { get; private set; }
        public float AvgPostReaction { get; private set; }

        public VkWall()
        {
            PostList = null;
            AvgPostReaction = 0;
        }

        public bool GetTopPosts(VkUser vkUser, int count)
        {
            int offset = 0;
            int postsReaction = 0;
            int postsCount;

            VkApiResponse<PostDTO> resp = new VkApi().GetOwnerPostsFromWall(vkUser.Id.ToString(), offset, count);

            if (resp.Response == null || resp.Response.Count == 0) return false;
            else
            {
                postsCount = resp.Response.Count > count ? count : resp.Response.Count;
                PostList = new List<VkPost>(postsCount);

                foreach (PostDTO post in resp.Response.Items)
                {
                    PostList.Add(new VkPost(post));
                    postsReaction += post.Likes.Count + post.Comments.Count + post.Reposts.Count;
                }

                AvgPostReaction = (float)postsReaction/postsCount;
                    
                return true;
            }
        }

        public bool GetUserWall(VkUser vkUser)
        {
            int offset = 0;
            int count = 100;

            VkApi ApiObj = new VkApi();

            VkApiResponse<PostDTO> resp = ApiObj.GetOwnerPostsFromWall(vkUser.Id.ToString(), offset, count);

            if (resp.Response == null || resp.Response.Count == 0) return false;
            else
            {
                PostList = new List<VkPost>(resp.Response.Count);

                for (int i = resp.Response.Count; i > 0; i -= 100)
                {
                    //Thread.Sleep(330);
                    foreach (PostDTO post in resp.Response.Items)
                        PostList.Add(new VkPost(post));

                    if (i > 100)
                    {
                        offset += 100;
                        resp = ApiObj.GetOwnerPostsFromWall(vkUser.Id.ToString(), offset, count);
                    }
                }
                return true;
            }
        }

        public bool GetNews(VkUser user)
        {
            if (user.FriendsList != null)
            {
                PostList = new List<VkPost>();

                foreach (VkUser friend in user.FriendsList)
                {
                    VkWall friendWall = new VkWall();

                    if (friendWall.GetUserWall(user))     // Если стена есть
                        foreach (VkPost post in friendWall.PostList)
                            PostList.Add(post);
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
