using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VkClientApp;

namespace VkDatabaseDll.Domain.Entity
{
    public class PostAttachment
    {
        public int Id { get; set; }
        public virtual Post Post { get; set; }

        public string Type { get; set; }
        public virtual Photo Photo { get; set; }
        public virtual Link Link { get; set; }
        
        public PostAttachment(VkPostAttachment vkPostAttachment)
        {
            Type = vkPostAttachment.Type;
            switch (Type)
            {
                case "photo":
                    Photo = new Photo(vkPostAttachment.Photo);
                    Link = null;
                    break;
                case "link":
                    Link = new Link(vkPostAttachment.Link);
                    Photo = null;
                    break;
                default:
                    Photo = null;
                    Link = null;
                    break;
            }
        }

        public PostAttachment() { }
    }
}