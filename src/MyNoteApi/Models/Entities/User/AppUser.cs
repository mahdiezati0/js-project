using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MyNoteApi.Models.Entities.User;

public class AppUser:IdentityUser
{
    [MaxLength(150)]
    public string? Name { get; set; }
}
