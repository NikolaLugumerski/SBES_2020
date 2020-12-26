using MimeKit;
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
using System.Configuration;
using System.Collections.Specialized;
using System.Windows;
using LiveCharts.Wpf;
using LiveCharts;

namespace PhishingApp.Commands
{
    public class SendEmailCommand : ICommand
    {
        

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        protected void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }

        private EmailModel emailModel;
        public EmailModel EmailModel
        {
            get { return emailModel; }
            set { emailModel = value; }
        }

        private StatisticsModel statisticsModel;

        public StatisticsModel StatisticsModel
        {
            get { return statisticsModel; }
            set { statisticsModel = value; }
        }

        private PieChartModel pieChartModel;

        public PieChartModel PieChartModel
        {
            get { return pieChartModel; }
            set { pieChartModel = value; }
        }



        public SendEmailCommand(EmailModel em, StatisticsModel sm, PieChartModel pcm)
        {
            EmailModel = em;
            StatisticsModel = sm;
            PieChartModel = pcm;
        }

        public bool CanExecute(object parameter)
        {
            if (EmailModel.Emails == null || EmailModel.SenderEmail == null || EmailModel.SenderName == null || 
                EmailModel.SenderPassword == null || EmailModel.RecipientName == null || EmailModel.Body == null || 
                EmailModel.EmailSubject == null)
            {
                return false;
            }

            return true;
        }
        
        public void Execute(object parameter)
        {
            string[] emailArray;
            emailArray = EmailModel.Emails.Split('\n');
            //Last string of array ends up being /n
            if(emailArray[emailArray.Length - 1] == "\n")
                Array.Resize(ref emailArray, emailArray.Length - 1);


            StatisticsModel.SentMails = emailArray.Length;
            PieChartModel.SentMailsSeries = new ChartValues<int>() { emailArray.Length };
            //when hitting enter in textbox \r is put
            for(int i=0; i<emailArray.Length; i++)
            {
                emailArray[i] = emailArray[i].Replace("\r\n", "").Replace("\r", "").Replace("\n", "");
            }

            string smtpHost = ConfigurationManager.AppSettings.Get("smtpHost");
            int smtpPort = Int32.Parse(ConfigurationManager.AppSettings.Get("smtpPort"));
            bool smtpUseSSL = Boolean.Parse(ConfigurationManager.AppSettings.Get("smtpUseSSL"));

            foreach (string email in emailArray)
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(EmailModel.SenderName, EmailModel.SenderEmail));
                message.To.Add(new MailboxAddress(EmailModel.RecipientName, email));
                message.Subject = EmailModel.EmailSubject;
                
                message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = EmailModel.Body
                };

                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                    client.Connect(smtpHost, smtpPort, smtpUseSSL); // mozda 465
                    // Note: only needed if the SMTP server requires authentication
                    client.Authenticate(EmailModel.SenderEmail, EmailModel.SenderPassword);

                    client.Send(message);
                    client.Disconnect(true);
                }

            }

            MessageBox.Show("Messages sent.");
        }
    }
}
