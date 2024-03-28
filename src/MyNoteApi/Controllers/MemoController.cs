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
    /// <summary>
    /// Create new note
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    /// <response code="201">Indicate that note created successfully</response>
    /// <response code="400">If model is not valid</response>
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
    /// <summary>
    /// Fetch note by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <response code="200">note detail</response>
    /// <response code="400">note not found or not accessible</response>
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
    /// <summary>
    /// Get all user's notes
    /// </summary>
    /// <param name="page"></param>
    /// <param name="size"></param>
    /// <returns></returns>
    /// <response code="200">All user's notes</response>
    /// <response code="400">user not access</response>
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
    /// <summary>
    /// Modify note
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    /// <response code="200">note changed</response>
    /// <response code="400">model is not valid</response>
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
    /// <summary>
    /// Delete note
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <response code="200">note removed</response>
    /// <response code="400">note not found/accessible </response>
    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var userId = _currentUserService.UserId;
        var result =await _memoService.DeleteMemo(userId, id);
        if (result.IsSuccess)
            return Ok(result.ToResult());
        return BadRequest(result.ToResult());
    }
}
