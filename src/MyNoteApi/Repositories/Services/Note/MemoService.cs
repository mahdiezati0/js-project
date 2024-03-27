using MyNoteApi.Data;
using MyNoteApi.Repositories.Interfaces.Note;

namespace MyNoteApi.Repositories.Services.Note;

public partial class MemoService : IMemoService
{
    private readonly AppDbContext _context;
    public MemoService(AppDbContext context) => _context = context;
}