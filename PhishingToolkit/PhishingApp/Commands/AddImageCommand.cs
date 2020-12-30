using MimeKit;
using MimeKit.Utils;
using PhishingApp.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;

namespace PhishingApp.Commands
{
    public class AddImageCommand : ICommand
    {
        private EmailModel emailModel;

        public EmailModel EmailModel
        {
            get { return emailModel; }
            set { emailModel = value; }
        }

        public AddImageCommand(EmailModel em)
        {
            EmailModel = em;
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
            return true;
        }

        public void Execute(object parameter)
        {
            string image = BrowseImageFiles();
            AddImageToBody(image);
        }

        public string BrowseImageFiles()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            dlg.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp";

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                return dlg.FileName;
            }

            return "error";
        }

        public void AddImageToBody(string path)
        {
            if (path == "error")
                return;


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

            var image = EmailModel.BodyBuilder.LinkedResources.Add(path);
            image.ContentId = MimeUtils.GenerateMessageId();

            EmailModel.HtmlBody += string.Format(@"{2} <center><img src=""cid:{1}""></center> {2}", EmailModel.Body, image.ContentId, Environment.NewLine);
            EmailModel.HtmlBodyHelper = EmailModel.HtmlBody;


            EmailModel.BodyBuilder.HtmlBody = EmailModel.HtmlBody;

            EmailModel.BodyBuilder.Attachments.Add(path);

            EmailModel.Body = EmailModel.BodyBuilder.HtmlBody;

       
        }

    }
}
