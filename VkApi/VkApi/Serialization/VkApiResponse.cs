using System.Runtime.Serialization;

namespace VkApiDll
{
    [DataContract]
    public class VkApiResponse<T>
    {
        [DataMember(Name = "response")]
        public VkApiResponseContent<T> response;
    }
    
    [DataContract]
    public class VkApiResponse
    {
        [DataMember(Name = "response")]
        public UserDTO[] response;
    }
}