using NetProject.Services;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Mvc;
using NetProject.Services;
using NetProject.Repository;
using NetProject.Models;

using Microsoft.EntityFrameworkCore;

namespace AuthService.Controllers;

[ApiController] 
[Route("api/[controller]")] // equivelant to requestmapping in String, is resolved to auth 
public class AuthController : ControllerBase
{
    private readonly tokenService _tokenService;
    private readonly IConfiguration _config;

    private readonly DbContext _dbContext;
    public AuthController(tokenService tokenService, IConfiguration config)
    {
        _tokenService = tokenService;
        _config = config;
    }


    

    [HttpPost("google")] // Maps to POST /api/auth/google
    public async Task<IActionResult> GoogleAuth([FromBody] GoogleAuthDto dto)
    {
        try
        {
            
            var googleClientId = _config["GoogleSettings:ClientId"];

            var settings = new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new[] { googleClientId }
            };

         
            var payload = await GoogleJsonWebSignature.ValidateAsync(dto.IdToken, settings);

            string userEmail = payload.Email;
            string googleUserId = payload.Subject;

            var user = await _dbContext.Users
             .FirstOrDefaultAsync(u => u.GoogleSubjectId == googleUserId);

         // 3. If user doesn't exist, register them
         if (user == null)
         {
             user = new User
             {
                 Email = userEmail,
                 GoogleSubjectId = googleUserId,
                 AuthProvider = "Google",
                 CreatedAt = DateTime.UtcNow
             };

             _dbContext.Users.Add(user);
             await _dbContext.SaveChangesAsync();
         }  


            
            string appToken = _tokenService.GenerateJwtToken(userEmail, googleUserId);

            // HTTP 200 OK
            return Ok(new { token = appToken });
        }
        catch (InvalidJwtException)
        {
            // HTTP 401 UNAUTHORIZED
            return Unauthorized("Invalid or expired Google Token.");
        }
    }
}

public record GoogleAuthDto(string IdToken);