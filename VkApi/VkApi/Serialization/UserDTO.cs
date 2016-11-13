using System.Runtime.Serialization;

namespace VkApiDll
{
    [DataContract]
    public class UserDTO
    {
        [DataMember(Name = "id")]
        public int Id { get; internal set; }

        [DataMember(Name = "first_name")]
        public string FirstName { get; internal set; }

        [DataMember(Name = "last_name")]
        public string LastName { get; internal set; }

        [DataMember(Name = "photo_50")]
        public string Photo50 { get; internal set; }
    }
}
