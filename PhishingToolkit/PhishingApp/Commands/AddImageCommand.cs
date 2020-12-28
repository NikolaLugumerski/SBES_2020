﻿using MimeKit;
using MimeKit.Utils;
using PhishingApp.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
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

            //var attachment = new MimePart("image", "gif")
            //{
            //    Content = new MimeContent(File.OpenRead(path), ContentEncoding.Default),
            //    ContentDisposition = new MimeKit.ContentDisposition(MimeKit.ContentDisposition.Attachment),
            //    ContentTransferEncoding = ContentEncoding.Base64,
            //    FileName = Path.GetFileName(path)
            //};

            //var body = new TextPart("plain") { Text = EmailModel.Body };


            //// now create the multipart/mixed container to hold the message text and the
            //// image attachment
            //var multipart = new Multipart("mixed");
            //multipart.Add(body);
            //multipart.Add(attachment);

            //EmailModel.MessageToSend.Body = multipart;

            var builder = new BodyBuilder();

            builder.TextBody = EmailModel.Body;

            var image = builder.LinkedResources.Add(path);
            image.ContentId = MimeUtils.GenerateMessageId();

            builder.HtmlBody = string.Format(@"<p>{0}</p> <center><img src=""cid:{1}""></center>", EmailModel.Body , image.ContentId);

            builder.Attachments.Add(path);

            EmailModel.Body = builder.HtmlBody;


            EmailModel.MessageToSend.Body = builder.ToMessageBody();
        }
      


    }
}
