using System.ComponentModel.DataAnnotations;

namespace MyNoteApi.Models.ViewModels.Note;

public record NewMemoViewModel([Required][MaxLength(200)] string title, string content);
