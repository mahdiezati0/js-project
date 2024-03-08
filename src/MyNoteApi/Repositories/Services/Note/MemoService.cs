using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyNoteApi.Data;
using MyNoteApi.Models.DataTransfareObject.Note;
using MyNoteApi.Models.Entities.Note;
using MyNoteApi.Models.Entities.User;
using MyNoteApi.Models.ViewModels.Note;
using MyNoteApi.Repositories.Interfaces.Note;

namespace MyNoteApi.Repositories.Services.Note;

public class MemoService : IMemoService
{
    private readonly AppDbContext _context;
    public MemoService(AppDbContext context) => _context = context;
    private async Task<Result<AppUser>> GetUserById(string userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user is null)
            return Result.Failure<AppUser>("user not found !");
        return user;
    }
    public async Task<Result<string>> CreateMemo(NewMemoDto model)
    {
        var user = await GetUserById(model.userId);
        if (user.IsFailure) return Result.Failure<string>(user.Error);
        var memo = new Memo
        {
            Content = model.content,
            Title = model.title,
            CreatedOn = DateTime.Now,
            IsDeleted = false,
            User = user.Value
        };
        await _context.Memos.AddAsync(memo);
        await _context.SaveChangesAsync();
        return memo.Id.ToString();
    }
    public async Task<Result<MemoDto>> GetMemoById(GetMemoDto model)
    {
        var user = await GetUserById(model.userId);
        if (user.IsFailure) return Result.Failure<MemoDto>(user.Error);

        var memo = await _context.Memos
            .SingleOrDefaultAsync(e => e.IsDeleted == false && e.Id == model.memoId.ToGuid() && e.User.Id == user.Value.Id);
        if (memo is null)
            return Result.Failure<MemoDto>("could not find note !");
        return new MemoDto
        {
            Content = memo.Content,
            Title = memo.Title,
            CreatedOn = memo.CreatedOn,
            ModifiedOn = memo.ModifiedOn,
            Id = memo.Id.ToString()
        };
    }

    public async Task<Result<IList<MemoDto>>> GetMemos(GetMemosDto model)
    {
        var user = await GetUserById(model.userId);
        if (user.IsFailure) return Result.Failure<IList<MemoDto>>(user.Error);

        var memos =_context.Memos
            .Include(e => e.User)
            .AsNoTracking()
            .Where(e => e.User.Id == model.userId)
            .Skip((model.page - 1) * model.size)
            .Take(model.size)
            .AsQueryable();
        var result = memos.Select(e => new MemoDto
        {
            Content = e.Content,
            Title = e.Title,
            CreatedOn = e.CreatedOn,
            ModifiedOn = e.ModifiedOn,
            Id = e.Id.ToString()
        }).ToList();
        return result;
    }
}
