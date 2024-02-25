using Microsoft.AspNetCore.Mvc;
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
}
