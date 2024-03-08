using CSharpFunctionalExtensions;
using MyNoteApi.Models.DataTransfareObject.Note;

namespace MyNoteApi.Repositories.Interfaces.Note;

public interface IMemoService
{
    Task<Result<string>> CreateMemo(NewMemoDto model);
}
