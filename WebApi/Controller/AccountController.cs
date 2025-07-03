using Domain.ApiResponse;
using Domain.DTOs.Account;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController(IAccountService accountService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginDTO login)
    {
        var response = await accountService.LoginAsync(login);
        return StatusCode((int)response.StatusCode, response);
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterDTO register)
    {
        var response = await accountService.RegisterAsync(register);
        return StatusCode((int)response.StatusCode, response);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> LogoutAsync()
    {
        var response = await accountService.LogoutAsync();
        return StatusCode((int)response.StatusCode, response);
    }
}