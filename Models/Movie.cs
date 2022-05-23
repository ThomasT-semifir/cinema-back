using System;
using System.Collections.Generic;

namespace cinema_back.Models
{
    public partial class Movie
    {
        public Movie()
        {
            Reviews = new HashSet<Review>();
        }

        public int Id { get; set; }
        public string? Title { get; set; }
        public int? Duration { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }
    }
}
