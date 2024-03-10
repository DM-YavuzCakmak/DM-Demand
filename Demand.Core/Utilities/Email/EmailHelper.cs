using System.Diagnostics.CodeAnalysis;
using System.Net.Mail;

namespace Demand.Core.Utilities.Email
{
    public class EmailHelper
    {
        public static void SendEmail([NotNull] List<string> tos, [NotNull] string title, [NotNull] string text)
        {
            MailMessage mail = new();
            SmtpClient smtpServer = new("smtp.office365.com");
            #region From
            mail.From = new("booking@demmuseums.com", "DEM Museums");
            #endregion
            foreach (var to in tos)
                mail.To.Add(to);

            mail.Subject = title;
            mail.Body = text;
            mail.IsBodyHtml = true;
            smtpServer.Port = 587;
            smtpServer.Credentials = new System.Net.NetworkCredential("booking@demmuseums.com", "Boo212**C");
            smtpServer.EnableSsl = true;
            smtpServer.Send(mail);
        }
    }
}
