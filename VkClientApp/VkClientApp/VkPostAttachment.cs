using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VkApiDll;

namespace VkClientApp
{
    class VkPostAttachment
    {
        private String _type;

        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public VkPostAttachment(PostAttachment postAttachment)
        {
            Type = postAttachment.Type;
        }

    }
}
