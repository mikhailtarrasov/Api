using System.Collections.Generic;
using VkClientApp;

namespace VkDatabaseDll.Domain.Entity
{
    public class User
    {
        public int VkId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Photo { get; set; }
        public virtual List<Post> Posts { get; set; }           // Список постов на своей стене
        public virtual List<User> Friends { get; set; }
        public virtual ICollection<Group> Groups { get; set; }

        public User(VkUser vkUser)
        {
            VkId = vkUser.Id;
            FirstName = vkUser.FirstName;
            LastName = vkUser.LastName;
            Photo = vkUser.Photo50;

            Posts = new List<Post>();
            Groups = new HashSet<Group>();
            Friends = new List<User>();
        }

        public User()
        {
            Posts = new List<Post>();
            Friends = new List<User>();
            Groups = new HashSet<Group>();
        }
    }
}
