using MimeKit;
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

            string temp = "<html><body>" + EmailModel.Body + "\n<br/>\n" + GetEmbeddedImage(path) + "<html><body>";

            EmailModel.Body = temp;
        }
       
        private string GetEmbeddedImage(String filePath)
        {
            string initialContent = "data:image/" + Path.GetExtension(filePath).TrimStart('.') + ";base64,";

            initialContent += GetBase64(filePath);

            initialContent = "<img src=" + initialContent + ">";
            return initialContent;
        }

        private string GetBase64(string filePath)
        {
            using (Image img = Image.FromFile(filePath))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    img.Save(ms, img.RawFormat);
                    byte[] imgBytes = ms.ToArray();
                    return Convert.ToBase64String(imgBytes);
                }
            }
        }


    }
}
