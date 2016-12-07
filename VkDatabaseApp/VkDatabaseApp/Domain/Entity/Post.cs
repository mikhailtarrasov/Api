using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VkDatabaseApp.Domain.Entity
{
    public class Post
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public int FromId { get; set; }
        public string Text { get; set; }
        public int CommentsCount { get; set; }
        public int LikesCount { get; set; }
        public int RepostsCount { get; set; }
        //public PostAttachment[] Attachments { get; set; }
    }
}
