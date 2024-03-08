using System.ComponentModel.DataAnnotations;

namespace MyNoteApi.Models.ViewModels.Note;

public record UpdateMemoTitleViewModel(string memoId, [Required][MaxLength(200)] string title);

