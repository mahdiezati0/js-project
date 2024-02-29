using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyNoteApi.Models.Entities.User;
using MyNoteApi.Models.ViewModels.User;
using MyNoteApi.Repositories.Interfaces.User;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
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
        if (!user.EmailConfirmed)
        {
            return Result.Failure<LoginResponseViewModel>("Please Confirm Email");
        }
        var refreshToken = GenerateRefreshToken();
        await _userManager.ReplaceClaimAsync(user, new Claim("RefreshToken", string.Empty), new Claim("RefreshToken", refreshToken));
        await _userManager.ReplaceClaimAsync(user, new Claim("RefreshTokenExpirationDate", DateTime.MinValue.ToString()), new Claim("RefreshTokenExpirationDate", DateTime.Now.AddMinutes(7).ToString()));
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
            RefreshToken = refreshToken,
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
            new Claim(ClaimTypes.Role,AppRoles.USER),
            new Claim("RefreshToken", string.Empty),
            new Claim("RefreshTokenExpirationDate", DateTime.MinValue.ToString())
        };
        await _userManager.AddClaimsAsync(user, userClaims);

        return Result.Success();
    }

    public async Task<Result<LoginResponseViewModel>> RefreshLogin(RefreshTokenViewModel model)
    {
        var tokenPrincipalResult = GetPrincipalFromExpiredToken(model.Token);
        if (tokenPrincipalResult.IsFailure)
            return Result.Failure<LoginResponseViewModel>(tokenPrincipalResult.Error);
        var id = tokenPrincipalResult.Value.FindFirstValue(ClaimTypes.NameIdentifier) ?? Guid.Empty.ToString();
        var user = await _userManager.FindByIdAsync(id);
        if (user is null)
        {
            return Result.Failure<LoginResponseViewModel>("User not found !");
        }
        var claims = await _userManager.GetClaimsAsync(user);
        var refreshToken = claims.SingleOrDefault(e => e.Type == "RefreshToken");
        if (refreshToken is null || model.Refresh != refreshToken.Value)
        {
            return Result.Failure<LoginResponseViewModel>("Token Not Valid !");
        }
        var refreshTokenExpirationDate = claims.SingleOrDefault(e => e.Type == "RefreshTokenExpirationDate");
        if (refreshTokenExpirationDate is null || !DateTime.TryParse(refreshTokenExpirationDate.Value, out var exdate))
        {
            return Result.Failure<LoginResponseViewModel>("Token Not Valid !");
        }
        if (exdate.CompareTo(DateTime.Now) < 0)
        {
            return Result.Failure<LoginResponseViewModel>("Token Not Valid !");
        }
        var newRefreshToken = GenerateRefreshToken();
        await _userManager.ReplaceClaimAsync(user, refreshToken, new Claim("RefreshToken", newRefreshToken));
        var token = GetToken(tokenPrincipalResult.Value.Claims.ToList());
        return new LoginResponseViewModel
        {
            RefreshToken = newRefreshToken,
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            ExpirationDate = token.ValidTo
        };
    }
    private Result<ClaimsPrincipal> GetPrincipalFromExpiredToken(string? token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"])),
            ValidateLifetime = false
        };
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                return Result.Failure<ClaimsPrincipal>("Token Not Valid !");
            }
            return principal;
        }
        catch (Exception)
        {
            return Result.Failure<ClaimsPrincipal>("Token Not Valid !");
        }
    }
    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public async Task<Result> ConfirmEmail(VerifyEmailViewModel model)
    {
        var user = await _userManager.FindByIdAsync(model.Id);
        if (user is null)
            return Result.Failure("User Not Found !");

        var token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(model.Token));
        var result = await _userManager.ConfirmEmailAsync(user, token);
        if (result.Succeeded)
            return Result.Success();
        return Result.Failure(string.Join('\n', result.Errors.Select(err => err.Description).ToList()));
    }

    public async Task<Result> SendConfirmEmail(ConfirmationViewModel model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user is null)
            return Result.Failure("User Not Found !");

        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

        // Send UserId and Token 
        return Result.Success();
    }

    public async Task<Result> ForgetPassword(ForgetPasswordViewModel model)
    {
        var user = await _userManager.FindByIdAsync(model.Id);
        if (user is null)
            return Result.Failure("User Not Found !");

        var token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(model.Token));
        var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);
        if (result.Succeeded)
            return Result.Success();
        return Result.Failure(string.Join('\n', result.Errors.Select(err => err.Description).ToList()));
    }

    public async Task<Result> SendForgetPasswordEmail(ConfirmationViewModel model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user is null)
            return Result.Failure("User Not Found !");

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

        // Send UserId and Token 
        return Result.Success();
    }
}