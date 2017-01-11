using System;
using System.Collections.Generic;

namespace VkDatabaseDll.Domain.Entity
{
    public class Group
    {
        public int Id { get; set; }
        public string ScreenName { get; set; }
        public virtual ICollection<User> MembersList { get; set; }

        public Group(String groupName)
        {
            ScreenName = groupName;
            MembersList = new HashSet<User>();
        }
        public Group()
        {
            MembersList = new HashSet<User>();
        }
    }
}
