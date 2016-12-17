using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VkDatabaseDll.Domain.Entity
{
    public class Group
    {
        public int Id { get; set; }
        public string ScreenName { get; set; }
        public virtual ICollection<User> MembersList { get; set; }

        public Group(String groupName)
        {
            this.ScreenName = groupName;
            this.MembersList = new HashSet<User>();
        }
        public Group()
        {
            this.MembersList = new HashSet<User>();
        }
    }
}
