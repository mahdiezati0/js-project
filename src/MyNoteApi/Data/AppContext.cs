using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyNoteApi.Models.Entities.User;

namespace MyNoteApi.Data;

public class AppDbContext:IdentityDbContext<AppUser,AppRole,string>
{
    public AppDbContext(DbContextOptions options) : base(options) { }
}
