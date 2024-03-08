using CSharpFunctionalExtensions;
using MyNoteApi.Data;
using MyNoteApi.Models.DataTransfareObject.Note;
using MyNoteApi.Models.Entities.Note;
using MyNoteApi.Repositories.Interfaces.Note;

namespace MyNoteApi.Repositories.Services.Note;

public class MemoService : IMemoService
{
    private readonly AppDbContext _context;
    public MemoService(AppDbContext context) => _context = context;

    public async Task<Result<string>> CreateMemo(NewMemoDto model)
    {
        var user = await _context.Users.FindAsync(model.userId);
        if (user is null)
            return Result.Failure<string>("user not found !");
        var memo = new Memo
        {
            Content = model.content,
            Title = model.content,
            CreatedOn = DateTime.Now,
            IsDeleted = false,
            User = user
        };
        await _context.Memos.AddAsync(memo);
        await _context.SaveChangesAsync();
        return memo.Id.ToString();
    }
}
