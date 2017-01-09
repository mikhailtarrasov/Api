using System.Collections.Generic;
using VkApiDll.Serialization;

namespace VkClientApp
{
    public class VkPost
    {
        public int Id { get; private set; }
        public int OwnerId { get; private set; }
        public int FromId { get; private set; }
        public string Text { get; private set; }
        public int CommentsCount { get; private set; }
        public int LikesCount { get; private set; }
        public int RepostsCount { get; private set; }
        public List<VkPostAttachment> Attachments { get; private set; }

        public VkPost(PostDTO post)
        {
            Id = post.Id;
            OwnerId = post.OwnerId;
            FromId = post.FromId;
            Text = post.Text;
            CommentsCount = post.Comments.Count;
            LikesCount = post.Likes.Count;
            RepostsCount = post.Reposts.Count;

            if (post.AttachmentsDto != null)
            {
                Attachments = new List<VkPostAttachment>();
                foreach (var attachment in post.AttachmentsDto)
                {
                    Attachments.Add(new VkPostAttachment(attachment));
                }
            }
        }
    }
}
