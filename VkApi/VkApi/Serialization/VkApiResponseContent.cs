using System.Runtime.Serialization;

namespace VkApiDll
{
    [DataContract]
    public class VkApiResponseContent<T>
    {
        [DataMember(Name = "count")] 
        private int _count;

        [DataMember(Name = "items")] 
        private T[] _items;

        public int Count
        {
            get { return _count; }
            set { _count = value; }
        }

        public T[] Items
        {
            get { return _items; }
            set { _items = value; }
        }
    }
}