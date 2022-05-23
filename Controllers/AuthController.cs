using cinema_back.Dto;
using cinema_back.Helpers;
using cinema_back.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace cinema_back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly cinemaContext _context;

        public AuthController(cinemaContext context)
        {
            _context = context;
        }

        // POST api/<AuthController>
        [HttpPost("login")]
        public ActionResult<AuthenticatedResponse> Login([FromBody] LoginDto user)
        {
            if (user == null)
            {
                return BadRequest("Invalid request");
            }
            User dbUser = CheckCredentials(user);
            if (dbUser != null)
            {
               AuthenticatedResponse response = JwtConfiguration.CreateJwt(dbUser);
                return Ok(response);
            }
            return Unauthorized(user);
        }

        private User CheckCredentials(LoginDto user)
        {
            return _context.Users.Where(predicate: u => u.Username == user.UserName && u.Password == user.Password).FirstOrDefault();
        }
    }
}
