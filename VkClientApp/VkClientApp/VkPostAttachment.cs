using System.Text;
using System.Windows.Media.Animation;
using VkApiDll.Serialization;

namespace VkClientApp
{
    public class VkPostAttachment
    {
        public string Type { get; private set; }
        public VkPhoto Photo { get; private set; }
        public VkLink Link { get; private set; }
        
        public VkPostAttachment(PostAttachmentDTO postAttachmentDto)
        {
            Type = postAttachmentDto.Type;
            switch (Type)
            {
                case "photo":
                    Photo = new VkPhoto(postAttachmentDto.PhotoDto);
                    Link = null;
                    break;
                case "link":
                    Link = new VkLink(postAttachmentDto.LinkDto);
                    Photo = null;
                    break;
                default:
                    Photo = null;
                    Link = null;
                    break;
            }
        }
    }

    public class VkPhoto
    {
        public int Id { get; set; }
        public string PhotoUrl { get; set; }

        public VkPhoto(PhotoDTO photoDto)
        {
            Id = photoDto.Id;
            PhotoUrl = photoDto.PhotoUrl;
        }
    }
    public class VkLink
    {
        public string Url { get; set; }
        public string Title { get; set; }

        public VkLink(LinkDTO linkDto)
        {
            Title = linkDto.Title;
            Url = linkDto.Url;
        }
    }
}