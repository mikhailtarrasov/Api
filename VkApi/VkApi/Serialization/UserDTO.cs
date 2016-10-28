using System.Runtime.Serialization;

namespace VkApiDll
{
    [DataContract]
    public class UserDTO
    {
        [DataMember(Name = "id")]
        private int id;

        [DataMember(Name = "first_name")] 
        private string first_name;

        [DataMember(Name = "last_name")] 
        private string last_name;

        [DataMember(Name = "photo_50")] 
        private string photo_50;
    }
}
