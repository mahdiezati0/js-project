using CSharpFunctionalExtensions;
using MyNoteApi.Models.ViewModels.User;

namespace MyNoteApi.Repositories.Interfaces.User;

public interface IUserService
{
    public Task<Result> Register(RegisterViewModel model);
    public Task<Result<LoginResponseViewModel>> Login(LoginViewModel model);
    public Task<Result<LoginResponseViewModel>> RefreshLogin(RefreshTokenViewModel model);
    public Task<Result> ConfirmEmail(VerifyEmailViewModel model);
    public Task<Result> SendConfirmEmail(ConfirmationViewModel model);
    public Task<Result> ForgetPassword(ForgetPasswordViewModel model);
    public Task<Result> SendForgetPasswordEmail(ConfirmationViewModel model);
}