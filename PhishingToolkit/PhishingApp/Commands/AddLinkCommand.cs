using PhishingApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.ServiceModel.Configuration;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PhishingApp.Commands
{
	public class AddLinkCommand : ICommand
	{
        private EmailModel emailModel;

        public EmailModel EmailModel
        {
            get { return emailModel; }
            set { emailModel = value; }
        }

        private ValidationModel validationModelAddLink;

        public ValidationModel ValidationModelAddLink
        {
            get { return validationModelAddLink; }
            set { validationModelAddLink = value; }
        }


        public AddLinkCommand(EmailModel emailModel, ValidationModel vm)
        {
            EmailModel = emailModel;
            ValidationModelAddLink = vm;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        protected void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }



        public bool CanExecute(object parameter)
        {
            ValidationModelAddLink.Text = string.Empty;
            string temp = "";
            if (EmailModel.LinkToAdd == string.Empty)
            {
                temp += "Link is empty" + "\n";
            }
            if (EmailModel.TextForLink == string.Empty)
            {
                temp += "Text for link is empty";
            }

            ValidationModelAddLink.Text = temp;



            if (EmailModel.LinkToAdd == "" || EmailModel.TextForLink == "")
                return false;

            return true;
        }


        public void Execute(object parameter)
        {
            if (!EmailModel.LinkToAdd.Contains("http://") && !EmailModel.LinkToAdd.Contains("https://"))
            {
                EmailModel.LinkToAdd = "http://" + EmailModel.LinkToAdd;
            }

            EmailModel.BodyBuilder.TextBody = EmailModel.Body;

            if (EmailModel.Body != null)
            {
                if (EmailModel.HtmlBody == null)
                {
                    EmailModel.HtmlBody = "\n" + "<p  style=\"white-space: pre-line\">" + EmailModel.Body + "</p>" + "\n";
                    EmailModel.HtmlBodyHelper = EmailModel.Body;

                    // za preview
                    EmailModel.HtmlForPreview = "\n" + "<p  style=\"white-space: pre-line\">" + EmailModel.Body + "</p>" + "\n";
                }
                else
                {
                    string temp = EmailModel.Body.Substring(EmailModel.HtmlBodyHelper.Length);

                    if (temp == "")
                    {

                    }
                    else
                    {
                        EmailModel.HtmlBody += "\n" + "<p  style=\"white-space: pre-line\">" + temp + " </p>" + "\n";
                        // za preview
                        EmailModel.HtmlForPreview += "\n" + "<p  style=\"white-space: pre-line\">" + temp + " </p>" + "\n";
                        EmailModel.HtmlBodyHelper = EmailModel.Body;
                    }
                }
            }

            EmailModel.HtmlBody += string.Format(@"<a href=""{0}""\>{1}</a>",EmailModel.LinkToAdd, EmailModel.TextForLink);

            // za preview
            EmailModel.HtmlForPreview += string.Format(@"<a href=""{0}"">{1}</a>", EmailModel.LinkToAdd, EmailModel.TextForLink);

            // zato sto prikazujes html u body mora ova linija koda
            EmailModel.HtmlBodyHelper = EmailModel.HtmlBody;


            EmailModel.BodyBuilder.HtmlBody = EmailModel.HtmlBody;
            EmailModel.Body = EmailModel.BodyBuilder.HtmlBody;
            
            
        }
    }
}
