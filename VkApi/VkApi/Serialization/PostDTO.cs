using System.Runtime.Serialization;

namespace VkApiDll
{
    [DataContract]
    public class PostDTO
    {
        [DataMember(Name = "id")]
        public int id;

        [DataMember(Name = "owner_id")]     /* идентификатор владельца стены, на которой размещена запись */
        public int owner_id;

        [DataMember(Name = "from_id")]      /* идентификатор автора записи */
        public int from_id;

        [DataMember(Name = "text")]       
        public string text;

        [DataMember(Name = "comments")]
        public PostReaction comments;

        [DataMember(Name = "likes")]
        public PostReaction likes;

        [DataMember(Name = "reposts")]
        public PostReaction reposts;

        [DataMember(Name = "PostAttachments")]
        public PostAttachments[] PostAttachments;
    }
}
