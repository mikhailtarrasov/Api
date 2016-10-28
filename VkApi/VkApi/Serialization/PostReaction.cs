using System.Runtime.Serialization;

namespace VkApiDll
{
    [DataContract]
    public class PostReaction
    {
        [DataMember(Name = "count")]
        private int _count;

        public int Count
        {
            get { return _count; }
            set { _count = value; }
        }
    }
}
