using System.Net;
using System.Net.Mail;
using System.Text;

namespace backend.Util;

public static class MailUtil
{
    public static bool SendMail(string to, string subject, string body)
    {
        return SendMail(new MailAddress(to), subject, body);
    }

    private static bool SendMail(MailAddress to, string subject, string body)
    {
        try
        {
            var smtpClient = new SmtpClient(Program.config["Mail:Host"],
                int.Parse(Program.config["Mail:Port"] ?? string.Empty));

            // set smtp-client with basicAuthentication
            smtpClient.UseDefaultCredentials = false;
            smtpClient.EnableSsl = true;
            smtpClient.Credentials =
                new NetworkCredential(Program.config["Mail:Address"], Program.config["Mail:Password"]);

            // add from / to mail addresses
            var from = new MailAddress(Program.config["Mail:Address"] ?? string.Empty, "Rezep");
            // MailAddress to = new MailAddress("test2@example.com", "TestToName");
            var mail = new MailMessage(from, to);

            // add ReplyTo
            // var replyTo = new MailAddress("reply@example.com");
            // mail.ReplyToList.Add(replyTo);

            // set subject and encoding
            mail.Subject = subject;
            mail.SubjectEncoding = Encoding.UTF8;

            // set body-message and encoding
            mail.Body = body;
            mail.BodyEncoding = Encoding.UTF8;
            // text or html
            mail.IsBodyHtml = true;

            smtpClient.Send(mail);
        }
        catch
        {
            return false;
        }

        return true;
    }
}