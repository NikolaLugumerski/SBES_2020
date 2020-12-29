using MimeKit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PhishingApp.Model
{
	public class EmailModel : INotifyPropertyChanged
	{
        private bool htmlImported;

        public bool HtmlImported
        {
            get { return htmlImported; }
            set
            { 
                htmlImported = value;
                OnPropertyChanged("HtmlImported");
            }
        }


        private string validate;

        public string Validate
        {
            get { return validate; }
            set 
            {
                validate = value;
                OnPropertyChanged("Validate");
            }
        }

        private MimeMessage messageToSend;

        public MimeMessage MessageToSend
        {
            get { return messageToSend; }
            set { messageToSend = value; }
        }



        private string emails;

        public string Emails
        {
            get { return emails; }
            set {
                emails = value;
                OnPropertyChanged("Emails");
            }
        }

        private string body;

        public string Body
        {
            get { return body; }
            set { 
                body = value;
                OnPropertyChanged("Body");
            }
        }

        private string maliciousLink;

        public string MaliciousLink
        {
            get { return maliciousLink; }
            set { 
                maliciousLink = value;
                OnPropertyChanged("MaliciousLink");
            }
        }



        private string senderName;

        public string SenderName
        {
            get { return senderName; }
            set { 
                senderName = value;
                OnPropertyChanged("SenderName");
            }
        }

        private string senderPassword;

        public string SenderPassword
        {
            get { return senderPassword; }
            set { 
                senderPassword = value;
                OnPropertyChanged("SenderPassword");
            }
        }


        private string senderEmail;

        public string SenderEmail
        {
            get { return senderEmail; }
            set { 
                senderEmail = value;
                OnPropertyChanged("SenderEmail");
            }
        }

        private string recipientName;

        public string RecipientName
        {
            get { return recipientName; }
            set { 
                recipientName = value;
                OnPropertyChanged("RecipientName");
            }
        }

        private string emailSubject;

        public string EmailSubject
        {
            get { return emailSubject; }
            set {
                emailSubject = value;
                OnPropertyChanged("EmailSubject");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

     
        public EmailModel()
        {
            MessageToSend = new MimeMessage();
        }
    }

}
