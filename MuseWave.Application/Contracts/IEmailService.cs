using MuseWave.Application.Models;

namespace MuseWave.Application.Contracts
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(Mail email);
    }
}