using System.Runtime.Serialization;

namespace VkApiDll
{
    [DataContract]
    public class PostDTO
    {
        [DataMember(Name = "id")]
        private int _id;
        public int Id
        {
            get { return _id; }
            internal set { _id = value; }
        }

        [DataMember(Name = "owner_id")] 
        private int _ownerId;

        [DataMember(Name = "from_id")] 
        private int _fromId;

        [DataMember(Name = "text")] 
        private string _text;

        [DataMember(Name = "comments")] 
        private PostReaction _comments;

        [DataMember(Name = "likes")] 
        private PostReaction _likes;

        [DataMember(Name = "reposts")] 
        private PostReaction _reposts;

        [DataMember(Name = "PostAttachments")] 
        private PostAttachment[] _attachments;


        public int OwnerId
        {
            get { return _ownerId; }
            internal set { _ownerId = value; }
        }

        public int FromId
        {
            get { return _fromId; }
            internal set { _fromId = value; }
        }

        public string Text
        {
            get { return _text; }
            internal set { _text = value; }
        }

        public PostReaction Comments
        {
            get { return _comments; }
            internal set { _comments = value; }
        }

        public PostReaction Likes
        {
            get { return _likes; }
            internal set { _likes = value; }
        }

        public PostReaction Reposts
        {
            get { return _reposts; }
            internal set { _reposts = value; }
        }

        public PostAttachment[] Attachments
        {
            get { return _attachments; }
            internal set { _attachments = value; }
        }
    }
}
