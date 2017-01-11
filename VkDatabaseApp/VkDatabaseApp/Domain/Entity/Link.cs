using VkClientApp;

namespace VkDatabaseDll.Domain.Entity
{
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