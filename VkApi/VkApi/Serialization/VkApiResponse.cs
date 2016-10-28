using System.Runtime.Serialization;

namespace VkApiDll
{
    [DataContract]
    public class VkApiResponse<T>
    {
        [DataMember(Name = "response")] 
        private VkApiResponseContent<T> _response;

        public VkApiResponseContent<T> Response
        {
            get { return _response; }
            set { _response = value; }
        }
    }
    
    [DataContract]
    public class VkApiResponse
    {
        [DataMember(Name = "response")] 
        private UserDTO[] _response;

        public UserDTO[] Response
        {
            get { return _response; }
            set { _response = value; }
        }
    }
}