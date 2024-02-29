using CSharpFunctionalExtensions;
using MyNoteApi.Models.ViewModels.User;

namespace MyNoteApi.Repositories.Interfaces.User;

public interface IUserService
{
    public Task<Result> Register(RegisterViewModel model);
    public Task<Result<LoginResponseViewModel>> Login(LoginViewModel model);
    public Task<Result<LoginResponseViewModel>> RefreshLogin(RefreshTokenViewModel model);
    public Task<Result> ConfirmEmail(VerifyEmailViewModel model);
    public Task<Result> SendConfirmEmail(ConfirmEmailViewModel model);
}