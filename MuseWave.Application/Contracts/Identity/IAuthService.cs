using MuseWave.Application.Models.Identity;

namespace MuseWave.Application.Contracts.Identity
{
    public interface IAuthService
    {
        Task<(int, string)> Registeration(RegistrationModel model, string role);
        Task<(int, string)> Login(LoginModel model);
        Task<(int, string)> Logout();
    }
}