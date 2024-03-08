using CSharpFunctionalExtensions;
using MyNoteApi.Models.DataTransfareObject.Note;

namespace MyNoteApi.Repositories.Interfaces.Note;

public interface IMemoService
{
    Task<Result<string>> CreateMemo(NewMemoDto model);
    Task<Result<MemoDto>> GetMemoById(GetMemoDto model);
    Task<Result<IList<MemoDto>>> GetMemos(GetMemosDto model);
}
