using System;
using System.Collections.Generic;

namespace cinema_back.Models
{
    public partial class User
    {
        public User()
        {
            Reviews = new HashSet<Review>();
        }

        public int Id { get; set; }
        public string Username { get; set; }
        public DateTime Birthdate { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public virtual ICollection<Review>? Reviews { get; set; }
    }
}
