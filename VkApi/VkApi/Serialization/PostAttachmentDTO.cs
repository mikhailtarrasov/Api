using System.Runtime.Serialization;

namespace VkApiDll.Serialization
{
    [DataContract]
    public class PostAttachmentDTO
    {
        [DataMember(Name = "type")]
        public string Type { get; internal set; }
        [DataMember(Name = "photo")]
        public PhotoDTO PhotoDto { get; internal set; }
        [DataMember(Name = "link")]
        public LinkDTO LinkDto { get; internal set; }
    }
}