using System.ComponentModel.DataAnnotations;

namespace MyNoteApi.Models.ViewModels.User;

public class ConfirmEmailViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}
