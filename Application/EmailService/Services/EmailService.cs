using Common.Dto;
using System.Net.Mail;
using System.Net;

namespace EmailService.Services
{

    public interface IEmailService
    {
        public Task<bool> SendEmail(EmailPackageDto emailPackageDto);
    }

    public class EmailService : IEmailService
    {
        public async Task<bool> SendEmail(EmailPackageDto emailPackageDto)
        {
            bool isSucces = false;

            // Create the SmtpClient
            SmtpClient Client = new SmtpClient()
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential()
                {
                    UserName = "dbassignmenttwopostgres@gmail.com",
                    Password = "ddujswjsnyfrvzrg"
                }
            };

            MailAddress FromEmail = new MailAddress("dbassignmenttwopostgres@gmail.com", "MTOGO System");
            MailAddress ToEmail = new MailAddress(emailPackageDto.Email, "MTOGO System");
            MailMessage Message = new MailMessage()
            {
                From = FromEmail,
                Subject = "DeliveryStatus",
                Body = emailPackageDto.Message
            };

            Message.To.Add(ToEmail);

            try
            {
                await Client.SendMailAsync(Message);
            }
            catch (System.Exception ex)
            {
                throw;
            }
            return isSucces;
        }
    }
}
