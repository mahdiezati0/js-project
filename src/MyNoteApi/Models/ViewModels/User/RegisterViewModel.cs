﻿using System.ComponentModel.DataAnnotations;

namespace MyNoteApi.Models.ViewModels.User;

public class RegisterViewModel
{
    [Required]
    [MaxLength(50)]
    public string Name { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    [MinLength(8)]
    public string Password { get; set; }
}
