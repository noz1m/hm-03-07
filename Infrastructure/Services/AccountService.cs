using Domain.ApiResponse;
using System.Net;
using System.Security.Claims;
using System.Text;
using Domain.DTOs.Account;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace Infrastructure.Services;

public class AccountService(DataContext context,UserManager<IdentityUser> userManager,IConfiguration config) : IAccountService
{
    public async Task<Response<string>> LoginAsync(LoginDTO login)
    {
        var user = await userManager.FindByNameAsync(login.Username);
        if (user == null)
            return new Response<string>("User not found", HttpStatusCode.NotFound);

        var result = await userManager.CheckPasswordAsync(user, login.Password);
        if (!result)
            return new Response<string>("Invalid username or password", HttpStatusCode.BadRequest);

        var token = GenerateJwtToken(user);
        return new Response<string>(token);
    }

    public async Task<Response<string>> RegisterAsync(RegisterDTO register)
    {
        var user = new IdentityUser
        {
            UserName = register.Username,
            PhoneNumber = register.PhoneNumber
        };

        var result = await userManager.CreateAsync(user, register.Password);
        if (!result.Succeeded)
        {
            var errors = string.Join("; ", result.Errors.Select(e => e.Description));
            return new Response<string>(errors, HttpStatusCode.BadRequest);
        }

        return new Response<string>("Successfully registered");
    }

    public Task<Response<string>> LogoutAsync()
    {
        return Task.FromResult(new Response<string>("Logout is handled client-side using JWT."));
    }

    private string GenerateJwtToken(IdentityUser user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Name, user.UserName!)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: config["Jwt:Issuer"],
            audience: config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}