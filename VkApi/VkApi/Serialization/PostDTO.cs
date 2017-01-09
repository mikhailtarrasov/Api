using System.Runtime.Serialization;

namespace VkApiDll.Serialization
{
    [DataContract]
    public class PostDTO
    {
        [DataMember(Name = "id")]
        public int Id { get; internal set; }

        [DataMember(Name = "owner_id")]
        public int OwnerId { get; internal set; }

        [DataMember(Name = "from_id")]
        public int FromId { get; internal set; }

        [DataMember(Name = "text")]
        public string Text { get; internal set; }

        [DataMember(Name = "comments")]
        public PostReaction Comments { get; internal set; }

        [DataMember(Name = "likes")]
        public PostReaction Likes { get; internal set; }

        [DataMember(Name = "reposts")]
        public PostReaction Reposts { get; internal set; }

        [DataMember(Name = "attachments")]
        public PostAttachmentDTO[] AttachmentsDto { get; internal set; }
    }
}
