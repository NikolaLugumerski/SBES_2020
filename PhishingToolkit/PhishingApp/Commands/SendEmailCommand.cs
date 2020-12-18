using PhishingApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

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
            string[] email;
            email = EmailModel.Emails.Split('\n');
            Array.Resize(ref email, email.Length - 1);

            //because gmail has protection against unknown clients signing in you must use https://www.google.com/settings/security/lesssecureapps on your account
            System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            client.Timeout = 10000;
            client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential("testzafishingaplikaciju@gmail.com", "sifratest123");
            System.Net.Mail.MailMessage mm = new System.Net.Mail.MailMessage("testzafishingaplikaciju@gmail.com", "testzafishingaplikaciju@gmail.com", "subject", "hello");
            mm.BodyEncoding = UTF8Encoding.UTF8;
            mm.DeliveryNotificationOptions = System.Net.Mail.DeliveryNotificationOptions.OnFailure;
            client.Send(mm);
        }


    }
}
