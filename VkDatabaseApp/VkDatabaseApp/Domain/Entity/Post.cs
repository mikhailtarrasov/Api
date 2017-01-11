using System.Collections.Generic;
using VkClientApp;

namespace VkDatabaseDll.Domain.Entity
{
    public class Post
    {
        public int Id { get; set; }
        public virtual User User { get; set; }
        //public virtual User FromUser { get; set; }
        public string Text { get; set; }
        public int CommentsCount { get; set; }
        public int LikesCount { get; set; }
        public int RepostsCount { get; set; }
        public virtual List<PostAttachment> Attachments { get; set; }
        public float AvgWallsPostReaction { get; set; }

        public Post(VkPost vkPost, float postReaction)  // Post from wall owner
        {
            //Id = vkPost.Id;
            //User = user;
            //FromUser = owner;
            Text = vkPost.Text;
            CommentsCount = vkPost.CommentsCount;
            LikesCount = vkPost.LikesCount;
            RepostsCount = vkPost.RepostsCount;
            AvgWallsPostReaction = postReaction;

            if(vkPost.Attachments != null)
            {
                Attachments = new List<PostAttachment>();
                foreach (var attachment in vkPost.Attachments)
                {
                    Attachments.Add(new PostAttachment(attachment));
                }
            }
        }

        public Post()
        {
            Attachments = new List<PostAttachment>();
        }
    }
}
