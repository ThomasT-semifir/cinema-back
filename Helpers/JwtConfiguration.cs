using cinema_back.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace cinema_back.Helpers
{
    public class JwtConfiguration
    {
        public static AuthenticatedResponse CreateJwt(User user)
        {
            List<Claim> claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim(ClaimTypes.Role, user.Role)
                    };
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ceci est la clé secrète"));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokenOptions = new JwtSecurityToken(
                issuer: "https://localhost:7105",
                audience: "https://localhost:7105",
                claims: claims,
                expires: DateTime.Now.AddMinutes(5),
                signingCredentials: signinCredentials
                );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return new AuthenticatedResponse { Token = tokenString };
        }
    }
}
