using System;
using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Configuration;

namespace FunctionBlobTrigger.Models
{
    public class EmailSender : ISender
    {
        public void Send( string emailTo, string subject, string body, IConfigurationRoot config)
        {

            var emailPassword = config.GetSection("Values:Password").Value;
            var emailFrom = config.GetSection("Values:Email").Value;
            var host = config.GetSection("Values:Host").Value;
            var port = int.Parse(config.GetSection("Values:Port").Value);

            MailAddress fromContact = new MailAddress(emailFrom);
            MailAddress toContact = new MailAddress(emailTo);

            using MailMessage mailMessage = new MailMessage(fromContact, toContact);
            using SmtpClient smtpClient = new SmtpClient();

            mailMessage.Subject = subject;
            mailMessage.Body = body;
            smtpClient.Host = host;
            smtpClient.Port = port;
            smtpClient.EnableSsl = true;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(fromContact.Address, emailPassword);

            try
            {
                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
