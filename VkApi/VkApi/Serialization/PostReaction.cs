using System.Runtime.Serialization;

namespace VkApiDll
{
    [DataContract]
    public class PostReaction
    {
        [DataMember(Name = "count")]
        public int Count { get; internal set; }
    }
}
