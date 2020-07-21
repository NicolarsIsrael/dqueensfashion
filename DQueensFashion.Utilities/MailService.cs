using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace DQueensFashion.Utilities
{
    public interface IMailService
    {
        Task SendMail(string to, string subject, string body, NetworkCredential credential, string from, string title);
        Task SendMailToMultiple(string to, string subject, string body, NetworkCredential credential,
           List<string> multipleCCmail, string from, string title);
    }

    public class MailService : IMailService
    {
        public async Task SendMail(string to, string subject, string body, NetworkCredential credential,string from, string title)
        {
            var message = new MailMessage();
            if (!isValidEmail(to))
                throw new FormatException("The to-email does not have a valid format");

            message.To.Add(new MailAddress(to));
            message.From = new MailAddress(from,title);
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;
            try
            {
                using (var smtp = new SmtpClient())
                {
                    smtp.Credentials = credential;
                    //smtp.Host = "smtp.gmail.com";
                    smtp.Host = "plesk4200.is.cc";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;

                    //smtp.SendCompleted += new SendCompletedEventHandler(Smtp_SendCompleted);

                    await Task.Run(() => smtp.Send(message));
                }
            }
            catch (Exception ex)
            {
                // Todo:: log mail sending failure and continue
                // log the exception first
                throw;
            }
        }

        public async Task SendMailToMultiple(string to, string subject, string body, NetworkCredential credential,
            List<string> multipleCCmail,string from, string title)
        {
            var message = new MailMessage();
            if (!isValidEmail(to))
                throw new FormatException("The to-email does not have a valid format");

            message.To.Add(new MailAddress(to));
            message.From = new MailAddress(from,title);
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = false;

            foreach (string emailAddress in multipleCCmail)
            {
                message.Bcc.Add(new MailAddress(emailAddress));
            }

            try
            {
                using (var smtp = new SmtpClient())
                {
                    smtp.Credentials = credential;
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;

                    //smtp.SendCompleted += new SendCompletedEventHandler(Smtp_SendCompleted);

                    await Task.Run(() => smtp.Send(message));
                }
            }
            catch (Exception ex)
            {
                // Todo:: log mail sending failure and continue
                // log the exception first
                throw;
            }
        }

        private bool isValidEmail(string email)
        {
            //// Todo:: complete logic
            //// use regex to validate email
            //// throw new NotImplementedException();
            //return true;
            try
            {
                MailAddress m = new MailAddress(email);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
