using MyNoteApi.Models.ViewModels.Note;

namespace MyNoteApi.Models.DataTransfareObject.Note;

public record UpdateMemoTitleDto(string userId, string memoId, string title) : UpdateMemoTitleViewModel(memoId, title);
