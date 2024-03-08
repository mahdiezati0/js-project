using Microsoft.AspNetCore.Identity;
using MyNoteApi.Models.Entities.Note;
using MyNoteApi.Models.Entities.User;
using System.Security.Claims;

namespace MyNoteApi.Data.Initial;

public class DatabaseInitializer : IDatabaseInitializer
{
    private readonly AppDbContext _context;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly UserManager<AppUser> _userManager;
    private readonly ILogger<DatabaseInitializer> _logger;

    public DatabaseInitializer(AppDbContext context, RoleManager<AppRole> roleManager, UserManager<AppUser> userManager, ILogger<DatabaseInitializer> logger)
    {
        _context = context;
        _roleManager = roleManager;
        _userManager = userManager;
        _logger = logger;
    }

    public void Initial()
    {
        var databaseExist = !_context.Database.EnsureCreated();
        if (databaseExist)
        {
            _logger.LogInformation("Database Created Before !");
            return;
        }
        _logger.LogWarning("Database Doesn't Exist ! Creating ...");

        CreateUserAndRole();
        CreateMemo();
        return;
    }
    private void CreateUserAndRole()
    {
        var identityResult = new IdentityResult();

        var userRole = new AppRole
        {
            Name = AppRoles.USER
        };
        identityResult = _roleManager.CreateAsync(userRole).GetAwaiter().GetResult();
        if (!identityResult.Succeeded)
        {
            _logger.LogError($"Role {AppRoles.USER} Creation Failed : " +
                string.Join('\n', identityResult.Errors.ToList()));
            return;
        }
        _logger.LogInformation($"Role {AppRoles.USER} Created !");
        identityResult = new IdentityResult();

        var userApp = new AppUser
        {
            Name = "Default User",
            Email = "user@example.com",
            UserName = "user@example.com",
            EmailConfirmed = true
        };
        identityResult = _userManager.CreateAsync(userApp, "A123456789a*").GetAwaiter().GetResult();
        if (!identityResult.Succeeded)
        {
            _logger.LogError($"User {userApp.Email} Creation Failed : " +
                string.Join('\n', identityResult.Errors.ToList()));
            return;
        }
        _logger.LogInformation($"User {userApp.Email} Created Successful");

        _userManager.AddToRoleAsync(userApp, AppRoles.USER).GetAwaiter().GetResult();
        _logger.LogInformation($"Adding Role {AppRoles.USER} To {userApp.Email} Successful");

        var claims = new Claim[]
        {
            new Claim(ClaimTypes.Email,userApp.Email),
            new Claim(ClaimTypes.NameIdentifier,userApp.Id),
            new Claim(ClaimTypes.Role,AppRoles.USER),
            new Claim("RefreshToken", string.Empty),
            new Claim("RefreshTokenExpirationDate", DateTime.MinValue.ToString())
        };
        _userManager.AddClaimsAsync(userApp, claims).GetAwaiter().GetResult();
        _logger.LogInformation($"Adding Default Claims To {userApp.Email} Was Successful");

    }
    private void CreateMemo()
    {
        _logger.LogInformation("Creating First Memo !");
        var user = _userManager.Users.First();
        var memo = new Memo
        {
            Content = "<p>Hello To MyNote</p>",
            CreatedOn=DateTime.Now,
            Title="Introduction",
            User=user
        };
        _context.Memos.Add(memo);
        _context.SaveChanges();
        _logger.LogInformation($"Memo with title : {memo.Title} Created Successfully!");
    }
}
