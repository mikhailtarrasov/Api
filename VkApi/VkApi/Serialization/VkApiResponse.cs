using System.Runtime.Serialization;

namespace VkApiDll.Serialization
{
    [DataContract]
    public class VkApiResponse<T>
    {
        [DataMember(Name = "response")]
        public VkApiResponseContent<T> Response { get; internal set; }
    }
    
    [DataContract]
    public class VkApiResponse
    {
        [DataMember(Name = "response")]
        public UserDTO[] Response { get; internal set; }
    }
}