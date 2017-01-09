using System.Runtime.Serialization;

namespace VkApiDll.Serialization
{
    [DataContract]
    public class LinkDTO
    {
        [DataMember(Name = "url")]
        public string Url { get; set; }
        [DataMember(Name = "title")]
        public string Title { get; set; } 
    }
}