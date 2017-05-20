using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Artelus.Common
{
    public static class Mail
    {
        public static void Send(string to, string subject, string body, bool isBodyHTML, List<string> files)
        {
            Send(string.Empty, string.Empty, to, subject, body, isBodyHTML, files);
        }

        static void Send(string from, string replyTo, string to, string subject, string body, bool isbodyHTML, List<string> files)
        {
            try
            {
                MailMessage msg = new MailMessage();
                if (!string.IsNullOrEmpty(from))
                {
                    msg.From = new MailAddress(from);
                }
                if (!string.IsNullOrEmpty(replyTo))
                {
                    msg.ReplyToList.Add(replyTo);
                }
                foreach (string email in to.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    msg.To.Add(email);
                }
                msg.Subject = subject;
                msg.Body = body;
                msg.IsBodyHtml = isbodyHTML;
                if (files.Count > 0)
                {
                    foreach (var file in files)
                    {
                        msg.Attachments.Add(new Attachment(file));
                    }
                }
                SmtpClient smtp = new SmtpClient();
                smtp.Send(msg);
            }
            catch (Exception exp)
            {
                //HttpContext.Current.Response.Write(exp.Message);
                Console.WriteLine(exp.Message);
            }
        }
    }
}
