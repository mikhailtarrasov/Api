using System.Collections.Generic;
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

    public class Photo
    {
        public int Id { get; set; }
        public string PhotoUrl { get; set; }
        public virtual List<PostAttachment> ListAttachmentsWithThisPhoto { get; set; } 

        public Photo(VkPhoto vkPhoto)
        {
            Id = vkPhoto.Id;
            PhotoUrl = vkPhoto.PhotoUrl;
        }

        public Photo() { }
    }

    public class Link
    {
        public int Id { get; set; }

        public string Url { get; set; }
        public string Title { get; set; }

        public Link(VkLink vkLink)
        {
            Title = vkLink.Title;
            Url = vkLink.Url;
        }

        public Link() { }
    }
}