using System.Runtime.Serialization;

namespace VkApiDll
{
    [DataContract]
    public class UserDTO
    {
        [DataMember(Name = "id")]
        private int _id;

        [DataMember(Name = "first_name")] 
        private string _firstName;

        [DataMember(Name = "last_name")] 
        private string _lastName;

        [DataMember(Name = "photo_50")] 
        private string _photo50;

        public int Id
        {
            get { return _id; }
            internal set { _id = value; }
        }

        public string FirstName
        {
            get { return _firstName; }
            internal set { _firstName = value; }
        }

        public string LastName
        {
            get { return _lastName; }
            internal set { _lastName = value; }
        }

        public string Photo50
        {
            get { return _photo50; }
            internal set { _photo50 = value; }
        }
    }
}
