using MimeKit;
using MailKit;
using PhishingApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using MailKit.Net.Smtp;

namespace PhishingApp.Commands
{
    public class SendEmailCommand : ICommand
    {
        private EmailModel emailModel;

        public event EventHandler CanExecuteChanged;

        public EmailModel EmailModel
        {
            get { return emailModel; }
            set { emailModel = value; }
        }

        public SendEmailCommand(EmailModel em)
        {
            EmailModel = em;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            string[] emailArray;
            emailArray = EmailModel.Emails.Split('\n');
            Array.Resize(ref emailArray, emailArray.Length - 1);

            foreach (string email in emailArray)
            {
               
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Petar test", "petartestovic@gmail.com"));
                message.To.Add(new MailboxAddress("Customer", email));
                message.Subject = "How you doin'? testu moj";

                message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = EmailModel.Body
                };

                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                    client.Connect("smtp.gmail.com", 465, true); // mozda 465

                    // Note: only needed if the SMTP server requires authentication
                    client.Authenticate("petartestovic@gmail.com", "petartestovic123");

                    client.Send(message);
                    client.Disconnect(true);
                }

            }
        }
    }
}
