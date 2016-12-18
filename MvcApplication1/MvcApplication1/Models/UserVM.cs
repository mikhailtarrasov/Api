using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Providers.Entities;

namespace MvcApplication1.Models
{
    public class UserVM
    {
        public int Id { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Photo50 { get; private set; }
        public List<User> FriendsList { get; private set; }

        
    }
}