using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CodeFirst.Helpers;
using CodeFirst.Models;
using CodeFirst.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CodeFirst.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController  : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly IDbService _dbService;

    public AuthController(IConfiguration configuration, IDbService dbService)
    {
        _config = configuration;
        _dbService = dbService;
    }


    //rejestracja nowaego uzytkowanika
    [HttpPost("register")]
    public async Task<IActionResult> Register(Login model)
    {
        var hashedPasswordAndSalt = Logger.GetHashedPasswordAndSalt(model.Password);

        var user = new User()
        {
            Login = model.Username,
            Password = hashedPasswordAndSalt.Item1,
            Salt = hashedPasswordAndSalt.Item2,
            RefreshToken = Logger.GenerateRefreshToken()
        };
        
            await _dbService.RegisterUser(user);
            return Ok(new { Message = "Registered new user" });
     
    }
    //login z zapisaniem tokena 
    [HttpPost("login")]
    public async Task< IActionResult >Login(Login model)
    {
        User user = await _dbService.GetUser(model.Username);

        if (user == null || user.Password != Logger.GetHashedPasswordWithSalt(model.Password, user.Salt))
        {
            return Unauthorized();
        }
        
        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]!)),
                SecurityAlgorithms.HmacSha256
            ),
            expires: DateTime.UtcNow.AddMinutes(15)
        );
        
        var tokenHandler = new JwtSecurityTokenHandler();
        string tokenRef=Logger.GenerateRefreshToken();
        var stringToken = tokenHandler.WriteToken(token);

        await _dbService.SetUserToken(user, tokenRef);
        
        return Ok(new LoginResp
        {
            Token = stringToken,
            RefreshToken = tokenRef
        });
        
    }
    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken(Token model)
    {
        User user = await _dbService.GetUserByToken(model.RefreshToken);
        if (user == null)
        {
            throw new SecurityTokenException("Invalid refresh token");
        }
        
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),  
            new Claim(ClaimTypes.Name, user.Login)
        };
        
        JwtSecurityToken token = new JwtSecurityToken(
            issuer: _config["Jwt:RefIssuer"],
            audience: _config["Jwt:RefAudience"],
            claims: claims,
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:RefKey"]!)),
                SecurityAlgorithms.HmacSha256
            ),
            expires: DateTime.UtcNow.AddMinutes(15)
        );
        
        string tokenRef=Logger.GenerateRefreshToken();

        await _dbService.SetUserToken(user, tokenRef);

        var tokenHandler = new JwtSecurityTokenHandler().WriteToken(token);

        return Ok(new
        {
            accessToken = tokenHandler, 
            refreshToken = tokenRef
        });
    }
}