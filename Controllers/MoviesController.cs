#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using cinema_back.Models;
using cinema_back.Dto;

namespace cinema_back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly cinemaContext _context;

        public MoviesController(cinemaContext context)
        {
            _context = context;
        }

        // GET: api/Movies
        [HttpGet]
        public IQueryable<MovieDto> GetMovies()
        {
            IQueryable<MovieDto> movies = from movie in _context.Movies
                         select new MovieDto()
                         {
                             Id = movie.Id,
                             Title = movie.Title,
                             Duration = (int)movie.Duration
                         };

            return movies;
        }

        [HttpGet("{id}/reviews")]
        public ActionResult<IQueryable<MovieWithReviewDto>> GetMoviesWithReview(int id)
        {
            IQueryable<MovieWithReviewDto> movieWithReview = from movie in _context.Movies
                                                    where movie.Id == id
                                                    select new MovieWithReviewDto()
                                                    {
                                                        Id = movie.Id,
                                                        Title = movie.Title,
                                                        Duration = (int)movie.Duration,
                                                        Reviews = (ICollection<ReviewDto>)(from review in _context.Reviews
                                                                                           where review.MoviesId == movie.Id
                                                                                           select new ReviewDto()
                                                                                           {
                                                                                               Comment = review.Comment,
                                                                                               Id = review.Id,
                                                                                               Note = review.Note,
                                                                                               Username = (from user in _context.Users
                                                                                                           where user.Id == review.UsersId
                                                                                                           select user.Username).Single()
                                                                                           })
                                                    };
            if (!movieWithReview.Any())
            {
                return NotFound();
            }
            return Ok(movieWithReview.First());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> GetMovie(int id)
        {
            var movie = await _context.Movies.FindAsync(id);

            if (movie == null)
            {
                return NotFound();
            }

            return movie;
        }

        // PUT: api/Movies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovie(int id, Movie movie)
        {
            if (id != movie.Id)
            {
                return BadRequest();
            }

            _context.Entry(movie).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Movies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Movie>> PostMovie(Movie movie)
        {
            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMovie", new { id = movie.Id }, movie);
        }

        // DELETE: api/Movies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }
    }
}
