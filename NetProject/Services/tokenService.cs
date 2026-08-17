

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;


namespace NetProject.Services
{
    public class tokenService
    {
        private IConfiguration _config;

        public tokenService(IConfiguration config)
        {
            this._config = config;
        }

        public string GenerateJwtToken(String email, String userID)
        {
            var jwtSettings = _config.GetSection("JwtSettings");
            var secretKey = jwtSettings["Secret"];
            var issuer = jwtSettings["issuer"];
            var audience = jwtSettings["AuthServiceUsers"];
            var expiryMinutes = double.Parse(jwtSettings["ExpiryInMinutes"]!);  
            
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,userID),
                new Claim(JwtRegisteredClaimNames.Email,email),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())

            };


            var token = new JwtSecurityToken(

                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiryMinutes), signingCredentials: creds
                );



            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }

}
