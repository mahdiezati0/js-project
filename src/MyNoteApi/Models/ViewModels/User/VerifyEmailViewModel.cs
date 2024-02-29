using System.ComponentModel.DataAnnotations;

namespace MyNoteApi.Models.ViewModels.User;

public class VerifyEmailViewModel
{
    [Required]
    public string Token { get; set; }
    public string Id { get; set; }
}
