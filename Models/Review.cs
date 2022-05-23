using System;
using System.Collections.Generic;

namespace cinema_back.Models
{
    public partial class Review
    {
        public int Id { get; set; }
        public string? Comment { get; set; }
        public int? Note { get; set; }
        public int UsersId { get; set; }
        public int MoviesId { get; set; }

        public virtual Movie Movies { get; set; } = null!;
        public virtual User Users { get; set; } = null!;
    }
}
