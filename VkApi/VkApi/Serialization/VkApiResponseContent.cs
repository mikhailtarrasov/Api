using System.Runtime.Serialization;

namespace VkApiDll.Serialization
{
    [DataContract]
    public class VkApiResponseContent<T>
    {
        [DataMember(Name = "count")]
        public int Count { get; internal set; }

        [DataMember(Name = "items")]
        public T[] Items { get; internal set; }
    }
}