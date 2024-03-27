using System.ComponentModel.DataAnnotations;

namespace MyNoteApi.Models.ViewModels.User;

public class RequestEmailViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public RequestEmailType TypeOfRequest { get; set; } = RequestEmailType.EmailConfirmation;
}
public enum RequestEmailType : int
{
    ForgetPassword = 1,
    EmailConfirmation = 2
}