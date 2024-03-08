using CSharpFunctionalExtensions;
using MyNoteApi.Models.DataTransfareObject.Note;
using MyNoteApi.Models.ViewModels.Note;

namespace MyNoteApi.Repositories.Interfaces.Note;

public interface IMemoService
{
    Task<Result<string>> CreateMemo(NewMemoDto model);
    Task<Result<MemoDto>> GetMemoById(GetMemoDto model);
}
