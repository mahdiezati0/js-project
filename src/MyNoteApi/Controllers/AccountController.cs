using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyNoteApi.Models.Entities.User;
using MyNoteApi.Models.ViewModels.User;
using MyNoteApi.Repositories.Interfaces.User;
using MyNoteApi.Repositories.Services;

namespace MyNoteApi.Controllers;
[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    private readonly IUserService _userService;

    public AccountController(IUserService userService)
    {
        _userService = userService;
    }
    [HttpPost("Register")]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        var result = await _userService.Register(model);
        if(result.IsSuccess)
            return Created(string.Empty, result.ToResult());
        return BadRequest(result.ToResult());
    }
    [HttpPost("Login")]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        var result = await _userService.Login(model);
        if (result.IsSuccess)
            return Ok(result.ToResult());
        return Unauthorized(result.ToResult());
    }
    [Authorize(Roles =AppRoles.USER)]
    [HttpGet("CheckAuthenticate")]
    public IActionResult CheckAuthenticate() => Ok(Result.Success().ToResult());
    [HttpPatch("RefreshToken")]
    public async Task<IActionResult> RefreshToken(RefreshTokenViewModel model)
    {
        var result = await _userService.RefreshLogin(model);
        if (result.IsSuccess)
            return Ok(result.ToResult());
        return Unauthorized(result.ToResult());
    }
}
