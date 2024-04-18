using RestAPI.Models.DTOs;

namespace RestAPI.Services.EmailService
{
    public interface IEmailService
    {
        void SendEmail(EmailDto request);
        
     
    }
}
