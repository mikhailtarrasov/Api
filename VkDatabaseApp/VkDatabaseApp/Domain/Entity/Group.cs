using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VkDatabaseApp.Domain.Entity
{
    public class Group
    {
        public int Id { get; set; }
        public string ScreenName { get; set; }
        public virtual List<User> MembersList { get; set; } 
    }
}
