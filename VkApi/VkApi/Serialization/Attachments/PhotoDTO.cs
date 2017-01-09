using System.Runtime.Serialization;

namespace VkApiDll.Serialization
{
    [DataContract]
    public class PhotoDTO
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }
        [DataMember(Name = "photo_130")]
        public string PhotoUrl { get; set; }
    }
}