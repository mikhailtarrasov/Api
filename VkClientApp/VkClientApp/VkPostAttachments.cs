using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VkApiDll;

namespace VkClientApp
{
    class VkPostAttachments
    {
        private String _type;

        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public VkPostAttachments(PostAttachments post)
        {
            _type = post.Type;
        }

    }
}
