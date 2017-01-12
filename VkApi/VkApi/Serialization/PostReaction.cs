using System.Runtime.Serialization;

namespace VkApiDll.Serialization
{
    [DataContract]
    public class PostReaction
    {
        [DataMember(Name = "count")]
        public int Count { get; internal set; }
    }
}
