using Microsoft.Extensions.Options;
using RecipeBook.Util;
using System;
using System.Net;
using System.Net.Mail;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace RecipeBook.Services
{
    public class EmailService : IEmailService
    {
        private string _siteName = "Recipe Book";

        private readonly EmailOptions _emailOptions;

        public EmailService(IOptions<EmailOptions> emailOptions)
        {
            _emailOptions = emailOptions.Value;
        }

        public async Task NewAccountAsync(string email, string callbackUrl)
        {
            await SendEmailAsync(
                email,
                $"{_siteName}: Account Created",
                $"An account has been created for you!<br /><br />" +
                $"Please confirm your account and reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a><br /><br />"
                );
        }

        public async Task ResetPasswordAsync(string email, string callbackUrl)
        {
            await SendEmailAsync(
                email,
                $"{_siteName}: Reset Password",
                $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>."
                );
        }

        public async Task ConfirmEmailAsync(string email, string callbackUrl)
        {
            await SendEmailAsync(
                email,
                $"{_siteName}: Confirm Your Email",
                $"Please confirm your email address by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>."
                );
        }

        public async Task ConfirmNewEmailAsync(string email, string callbackUrl)
        {
            await SendEmailAsync(
                email,
                $"{_siteName}: Confirm Your New Email",
                $"Please confirm your new email address by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>."
                );
        }


        private Task SendEmailAsync(string email, string subject, string message)
        {

            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient smtpServer = new SmtpClient(_emailOptions.SmtpHost);

                mail.From = new MailAddress(_emailOptions.SenderAddress, _emailOptions.SenderName);
                mail.To.Add(email);
                mail.Subject = subject;
                mail.Body = message;
                mail.IsBodyHtml = true;

                smtpServer.Port = _emailOptions.Port;
                smtpServer.Credentials = new NetworkCredential(_emailOptions.SenderAddress, _emailOptions.SenderPassword);
                smtpServer.EnableSsl = true;

                smtpServer.Send(mail);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }

            return Task.CompletedTask;
        }
    }
}
