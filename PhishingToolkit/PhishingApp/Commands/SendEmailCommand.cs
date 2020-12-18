﻿using PhishingApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
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
            string[] emailArray;
            emailArray = EmailModel.Emails.Split('\n');
            Array.Resize(ref emailArray, emailArray.Length - 1);

            foreach (string email in emailArray)
            {
                //because gmail has protection against unknown clients signing in you must use https://www.google.com/settings/security/lesssecureapps on your account
                SmtpClient client = new SmtpClient();
                client.Port = 2525;
                client.Host = "smtp.mailtrap.io";
                client.Timeout = 10000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential("25accfc8259338", "4d71884d65d591");
                client.EnableSsl = true;
                
                MailMessage mm = new MailMessage("from@example.com", "testzafishingaplikaciju@gmail.com", "subject", EmailModel.Body);
                mm.BodyEncoding = UTF8Encoding.UTF8;
                mm.DeliveryNotificationOptions = System.Net.Mail.DeliveryNotificationOptions.OnFailure;
                client.Send(mm);
            }
        }
    }
}
