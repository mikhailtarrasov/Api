using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VkApiDll;

namespace VkClientApp
{
    public class VkPostAttachment
    {
        public string Type { get; set; }

        public VkPostAttachment(PostAttachment postAttachment)
        {
            Type = postAttachment.Type;
        }

    }
}
