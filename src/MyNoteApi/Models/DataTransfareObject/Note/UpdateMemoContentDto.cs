using MyNoteApi.Models.ViewModels.Note;

namespace MyNoteApi.Models.DataTransfareObject.Note;

public record UpdateMemoContentDto(string userId, string memoId, string content) : UpdateMemoContentViewModel(memoId, content);
