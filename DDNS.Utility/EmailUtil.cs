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

        /// <summary>
        /// 生成邮件内容
        /// </summary>
        /// <param name="name"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        private string GenerateMessageBody(string name, string address)
        {
            var tempHtml = "<p>{0}</p>";
            var body = string.Empty;

            var url = _email.Domain + "/account/reset?token=" + MD5Util.TextToMD5(address);
            var link = "<a href='" + url + "'>" + url + "</a>";

            body += string.Format(tempHtml, "尊敬的用户：");
            body += string.Format(tempHtml, name + "，您好！");
            body += string.Format(tempHtml, "你通过邮箱发起了找回密码操作，请点击以下链接根据页面提示进行密码找回。" + link);
            body += string.Format(tempHtml, "如果以上链接无法点击，请将上面的地址复制到你的浏览器的地址栏。");
            body += string.Format(tempHtml, "（该链接在48小时内有效，48小时后需要重新提交修改）");
            body += string.Format(tempHtml, "如非你本人操作，请忽略此邮件。");

            return body;
        }
    }
}