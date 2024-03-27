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
            return Created(string.Empty, result.ToResult());
        return BadRequest(result.ToResult());
    }
    [HttpGet("Get/{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var userId = _currentUserService.UserId;
        var request = new GetMemoDto(userId, id);
        var result = await _memoService.GetMemoById(request);
        if (result.IsSuccess)
            return Ok(result.ToResult());
        return BadRequest(result.ToResult());
    }
    [HttpGet("Get/{page}/{size}")]
    public async Task<IActionResult> GetById(int page = 1, int size = 5)
    {
        var userId = _currentUserService.UserId;
        var request = new GetMemosDto(userId, page, size);
        var result = await _memoService.GetMemos(request);
        if (result.IsSuccess)
            return Ok(result.ToResult());
        return BadRequest(result.ToResult());
    }
    [HttpPatch("Update")]
    public async Task<IActionResult> UpdateContent(UpdateMemoViewModel model)
    {
        var userId = _currentUserService.UserId;
        var request = new UpdateMemoDto(userId, model.memoId, model.title, model.content);
        var result = await _memoService.ModifyMemo(request);
        if (result.IsSuccess)
            return Ok(result.ToResult());
        return BadRequest(result.ToResult());
    }

}
