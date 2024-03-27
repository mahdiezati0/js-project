using System.ComponentModel.DataAnnotations;

namespace MyNoteApi.Models.ViewModels.User;

public class VerifyEmailViewModel
{
    [Required]
    public string Code { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}
