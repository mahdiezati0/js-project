using Microsoft.IdentityModel.Tokens;
using MyNoteApi.Repositories.Interfaces.User;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace MyNoteApi.Repositories.Services.User;

public partial class UserService : IUserService
{
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
}
