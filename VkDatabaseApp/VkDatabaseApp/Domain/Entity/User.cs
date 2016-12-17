using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VkClientApp;

namespace VkDatabaseDll.Domain.Entity
{
    public class User
    {
        public int VkId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Photo { get; set; }
        public virtual ICollection<User> Friends { get; set; }
        public virtual ICollection<Group> Groups { get; set; }

        public User(VkUser vkUser)
        {
            this.VkId = vkUser.Id;
            this.FirstName = vkUser.FirstName;
            this.LastName = vkUser.LastName;
            this.Photo = vkUser.Photo50;

            this.Groups = new HashSet<Group>();
            this.Friends = new HashSet<User>();
        }

        public User()
        {
            this.Friends = new HashSet<User>();
            this.Groups = new HashSet<Group>();
        }
    }
}
