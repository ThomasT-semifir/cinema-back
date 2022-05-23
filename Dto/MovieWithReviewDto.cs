using cinema_back.Models;

namespace cinema_back.Dto
{
    public class MovieWithReviewDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Duration { get; set; }
        public ICollection<ReviewDto> Reviews { get; set; }
    }
}
