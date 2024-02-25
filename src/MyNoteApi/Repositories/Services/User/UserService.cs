using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyNoteApi.Models.Entities.User;
using MyNoteApi.Models.ViewModels.User;
using MyNoteApi.Repositories.Interfaces.User;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyNoteApi.Repositories.Services.User;

public class UserService : IUserService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IConfiguration _configuration;

    public UserService(UserManager<AppUser> userManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }

    public async Task<Result<LoginResponseViewModel>> Login(LoginViewModel model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user is null)
        {
            return Result.Failure<LoginResponseViewModel>("User not found !");
        }
        if (!await _userManager.CheckPasswordAsync(user, model.Password))
        {
            return Result.Failure<LoginResponseViewModel>("Wrong password !");
        }
        var userRoles = await _userManager.GetRolesAsync(user);
        var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.NameIdentifier, user.Id)
        };
        foreach (var userRole in userRoles)
        {
            authClaims.Add(new Claim(ClaimTypes.Role, userRole));
        }
        var token = GetToken(authClaims);
        var result = new LoginResponseViewModel
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            ExpirationDate = token.ValidTo
        };
        return Result.Success(result);
    }
    private JwtSecurityToken GetToken(List<Claim> authClaims)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
        var defaultExTime = !int.TryParse(_configuration["JWT:TokenExpirationMinutes"], out var validTokenInMinutes);
        var token = new JwtSecurityToken(
            issuer: _configuration["JWT:Issuer"],
            audience: _configuration["JWT:Audience"],
            expires: DateTime.Now.AddMinutes(defaultExTime ? 5 : validTokenInMinutes),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

        return token;
    }
    public async Task<Result> Register(RegisterViewModel model)
    {
        var isEmailTaken = await _userManager.Users.AnyAsync(e => e.NormalizedEmail == model.Email.ToUpper());
        if (isEmailTaken)
            return Result.Failure($"Email {model.Email} Taken Use Another !");
        var user = new AppUser
        {
            Email = model.Email,
            UserName = model.Email,
            Name = model.Name
        };
        var registrationResult = await _userManager.CreateAsync(user, model.Password);
        if (!registrationResult.Succeeded)
        {
            return Result.Failure(string.Join('\n', registrationResult.Errors.Select(err => err.Description).ToList()));
        }
        await _userManager.AddToRoleAsync(user, AppRoles.USER);
        var userClaims = new Claim[]
        {
            new Claim(ClaimTypes.Email,user.Email),
            new Claim(ClaimTypes.NameIdentifier,user.Id),
            new Claim(ClaimTypes.Role,AppRoles.USER)
        };
        await _userManager.AddClaimsAsync(user, userClaims);
        // Send Verification Email ...

        return Result.Success();
    }
}