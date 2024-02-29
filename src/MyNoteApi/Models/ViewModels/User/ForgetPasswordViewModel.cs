using System.ComponentModel.DataAnnotations;

namespace MyNoteApi.Models.ViewModels.User;

public class ForgetPasswordViewModel
{
    [Required]
    public string Id { get; set; }
    [Required]
    public string Token { get; set; }
    [Required]
    [MinLength(8)]
    public string NewPassword { get; set; }
}
