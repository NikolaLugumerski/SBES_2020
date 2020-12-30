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
using MailKit.Security;
using System.IO;
using System.Globalization;
using System.ServiceModel.Channels;
using MailKit.Net.Smtp;

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
            if (EmailModel.Emails == null || EmailModel.SenderEmail.Equals("") || EmailModel.SenderName.Equals("") || 
                EmailModel.SenderPassword.Equals("") || EmailModel.RecipientName.Equals("") || EmailModel.Body == null || 
                EmailModel.EmailSubject.Equals(""))
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

          
            //when hitting enter in textbox \r is put
            for (int i=0; i<emailArray.Length; i++)
            {
                emailArray[i] = emailArray[i].Replace("\r\n", "").Replace("\r", "").Replace("\n", "");
            }

            string smtpHost = ConfigurationManager.AppSettings.Get("smtpHost");
            int smtpPort = Int32.Parse(ConfigurationManager.AppSettings.Get("smtpPort"));
            bool smtpUseSSL = Boolean.Parse(ConfigurationManager.AppSettings.Get("smtpUseSSL"));

          

            foreach (string email in emailArray)
            {
                if (!email.Equals(""))
                {
                    if (!IsValidEmail(email))
                    {
                        MessageBox.Show("Not all mails in the list are in a valid email format!");
                        return;
                    }
                }
            }


            if (EmailModel.HtmlImported)
            {
                EmailModel.MessageToSend.Body = new TextPart("plain") { Text = EmailModel.Body };
            }
            else
            {
                EmailModel.BodyBuilder.TextBody = EmailModel.Body;


                if (EmailModel.HtmlBody == null)
                {
                    EmailModel.HtmlBody = "\n" + "<p>" + EmailModel.Body + "</p>" + "\n";
                    EmailModel.HtmlBodyHelper = EmailModel.Body;
                }
                else
                {
                    string temp = EmailModel.Body.Substring(EmailModel.HtmlBodyHelper.Length);

                    if (temp == "")
                    {

                    }
                    else
                    {
                        EmailModel.HtmlBody += "\n" + "<p>" + temp + " </p>" + "\n";

                        EmailModel.HtmlBodyHelper = EmailModel.Body;
                    }
                }

                // zato sto prikazujes html u body mora ova linija koda
                EmailModel.HtmlBodyHelper = EmailModel.HtmlBody;


                EmailModel.BodyBuilder.HtmlBody = EmailModel.HtmlBody;
                EmailModel.Body = EmailModel.BodyBuilder.HtmlBody;


                EmailModel.MessageToSend.Body = EmailModel.BodyBuilder.ToMessageBody();

            }



            foreach (string email in emailArray)
            {
                EmailModel.MessageToSend.From.Add(new MailboxAddress(EmailModel.SenderName, EmailModel.SenderEmail));
                EmailModel.MessageToSend.To.Add(new MailboxAddress(EmailModel.RecipientName, email));
                EmailModel.MessageToSend.Subject = EmailModel.EmailSubject;


                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                    client.Connect(smtpHost, smtpPort, smtpUseSSL); // mozda 465
                    // Note: only needed if the SMTP server requires authentication
                    try
                    {
                        client.Authenticate(EmailModel.SenderEmail, EmailModel.SenderPassword);
                       
                    }
                    catch (AuthenticationException)
                    {
                        EmailModel.Validate = "Invalid email or password, try again";
                        MessageBox.Show("Invalid email or password, try again");
                        return;
                    }

                    //javlja se ovaj exception i svakako posalje poruku nzm sta je problem bogo
                    try
                    {
                        client.Send(EmailModel.MessageToSend);

                    }
                    catch (SmtpCommandException) { }

                    client.Disconnect(true);
                }

            }

            //mozda treba mozda ne
            StatisticsModel.SentMails = emailArray.Length - 1;


            FileStream fsOverwrite = new FileStream("sentMails.txt", FileMode.Create);
            using (StreamWriter sw = new StreamWriter(fsOverwrite))
            {
                sw.WriteLine(StatisticsModel.SentMails.ToString());
            }
            fsOverwrite.Close();

            PieChartModel.SentMailsSeries = new ChartValues<int>() { emailArray.Length };

            MessageBox.Show("Messages sent.");
        }


        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                Console.WriteLine(e);
                return false;
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e);
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
    }
}
