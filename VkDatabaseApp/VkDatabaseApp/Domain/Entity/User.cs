using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VkClientApp;

namespace VkDatabaseApp.Domain.Entity
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Photo { get; set; }
        public virtual List<User> Friends { get; set; }
        public virtual List<Group> Groups { get; set;  }
    }
}
