using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyNoteApi.Models.DataTransfareObject.Note;
using MyNoteApi.Models.Entities.User;
using MyNoteApi.Models.ViewModels.Note;
using MyNoteApi.Repositories.Interfaces.Note;
using MyNoteApi.Repositories.Interfaces.User;
using MyNoteApi.Repositories.Services;

namespace MyNoteApi.Controllers;
[ApiController]
[Route("[controller]")]
[Authorize(Roles = AppRoles.USER)]
public class MemoController : ControllerBase
{
    private readonly IMemoService _memoService;
    private readonly ICurrentUserService _currentUserService;
    public MemoController(IMemoService memoService, ICurrentUserService currentUserService)
    {
        _memoService = memoService;
        _currentUserService = currentUserService;
    }
    [HttpPost("New")]
    public async Task<IActionResult> Create(NewMemoViewModel model)
    {
        var userId = _currentUserService.UserId;
        var request = new NewMemoDto(userId, model.title, model.content);
        var result = await _memoService.CreateMemo(request);
        if (result.IsSuccess)
            return Created(string.Empty,result.ToResult());
        return BadRequest(result.ToResult());
    }
}
