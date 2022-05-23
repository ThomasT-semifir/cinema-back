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
using Microsoft.AspNetCore.Authorization;

namespace cinema_back.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly cinemaContext _context;

        public UsersController(cinemaContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetUsers()
        {
            var users = await _context.Users.Select(u =>
                new UserDto
                {
                    Id = u.Id,
                    Birthdate = u.Birthdate,
                    Username = u.Username
                }
                ).ToListAsync();

            return Ok(users);
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        [ActionName("GetUserById")]
        public async Task<ActionResult<UserDto>> GetUserById(int id)
        {
            var foundUser = await _context.Users.Where(u => u.Id == id).Select(u => 
                new UserDto { 
                    Id = u.Id, 
                    Birthdate = u.Birthdate, 
                    Username = u.Username
                    }).SingleOrDefaultAsync();
                

            if (foundUser == null)
            {
                return NotFound();
            }

            return Ok(foundUser);
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]

        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserDto>> PostUserAsync([FromBody] UserDto user)
        {
            if (UserExists(user.Username))
            {
                return BadRequest("user already exists");
            }

            User newUser = new User { Birthdate = user.Birthdate, Username = user.Username };
            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUsers", new { id = newUser.Id }, newUser);
 

        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

        private bool UserExists(string username)
        {
            return _context.Users.Any(e => e.Username == username);
        }
    }
}
