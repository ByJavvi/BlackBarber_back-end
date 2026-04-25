using BlackBarberAPI.DTOs;

namespace BlackBarberAPI.Services.Contratos
{
    public interface IEmailService
    {
        public Task EnviarEmail(EmailDTO emailDTO);
    }
}
