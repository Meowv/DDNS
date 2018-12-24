using DDNS.Entity.AppSettings;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace DDNS.Utility
{
    public class EmailUtil
    {
        private readonly EmailConfig _email;

        public EmailUtil(IOptions<EmailConfig> eamil)
        {
            _email = eamil.Value;
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="name"></param>
        /// <param name="address"></param>
        public void SendEmail(string name, string address, string subject, string body)
        {
            var message = new MimeMessage();

            message.From.Add(new MailboxAddress(_email.Name, _email.Address));
            message.To.Add(new MailboxAddress(name, address));
            message.Subject = subject;
            message.Body = new BodyBuilder
            {
                HtmlBody = body
            }.ToMessageBody();

            using (var client = new SmtpClient())
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                client.Connect(_email.Host, _email.Port, _email.UseSsl);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate(_email.UserName, _email.Password);
                client.Send(message);
                client.Disconnect(true);
            }
        }
    }
}