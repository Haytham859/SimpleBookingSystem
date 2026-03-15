using Microsoft.IdentityModel.Tokens;
using ProjectWithOutArck.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProjectWithOutArck.Configurations
{
    public static class GenerateToken
    {

        public static string GenerateJwt(User user,IConfiguration config)
        {
            var JwtSetting = config.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSetting["Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var Claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.Role,user.Role)
            };
            var token = new JwtSecurityToken(
                issuer: JwtSetting["Issuer"],
                audience: JwtSetting["Audience"],
                claims: Claims,
                expires: DateTime.Now.AddMinutes(double.Parse(JwtSetting["DurationInMinutes"])),
                signingCredentials: creds
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
