using System.ComponentModel.DataAnnotations;

namespace MyNoteApi.Models.ViewModels.User;

public class ForgetPasswordViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public string Code { get; set; }
    [Required]
    [MinLength(8)]
    public string NewPassword { get; set; }
}
