using System;
using System.Runtime.Serialization;

namespace VkApiDll
{
    [DataContract]
    public class PostAttachments
    {
        [DataMember(Name = "type")]
        public String type;

        // TODO this class
    }
}