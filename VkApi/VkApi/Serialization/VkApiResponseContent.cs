using System.Runtime.Serialization;

namespace VkApiDll
{
    [DataContract]
    public class VkApiResponseContent<T>
    {
        [DataMember(Name = "count")]
        public int count;

        [DataMember(Name = "items")]
        public T[] items;
    }
}