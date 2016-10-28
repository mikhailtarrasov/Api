using System;
using System.Runtime.Serialization;

namespace VkApiDll
{
    [DataContract]
    public class PostAttachments
    {
        [DataMember(Name = "type")] 
        private String _type;

        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }

        // TODO this class
    }
}