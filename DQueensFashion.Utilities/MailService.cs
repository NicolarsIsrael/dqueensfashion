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
        Task SendMail(string to, string subject, string body, NetworkCredential credential);
    }

    public class MailService : IMailService
    {
        public async Task SendMail(string to, string subject, string body, NetworkCredential credential)
        {
            var message = new MailMessage();
            if (!isValidEmail(to))
                throw new FormatException("The to-email does not have a valid format");

            message.To.Add(new MailAddress(to));
            message.From = new MailAddress(AppConstant.HDQ_EMAIL_ACCOUNT, AppConstant.HDQ_EMAIL_TITLE);
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;
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

        public async Task SendMailToMultiple(string to, string subject, string body, NetworkCredential credential, List<string> multipleCCmail)
        {
            var message = new MailMessage();
            if (!isValidEmail(to))
                throw new FormatException("The to-email does not have a valid format");

            message.To.Add(new MailAddress(to));
            message.From = new MailAddress(AppConstant.HDQ_EMAIL_ACCOUNT, AppConstant.HDQ_EMAIL_TITLE);
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = false;

            foreach (string emailAddress in multipleCCmail)
            {
                message.CC.Add(new MailAddress(emailAddress));

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
