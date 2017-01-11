using VkClientApp;

namespace VkDatabaseDll.Domain.Entity
{
    public class Photo
    {
        public int Id { get; set; }
        public string PhotoUrl { get; set; }
        
        public Photo(VkPhoto vkPhoto)
        {
            PhotoUrl = vkPhoto.PhotoUrl;
        }

        public Photo() { }
    }
}