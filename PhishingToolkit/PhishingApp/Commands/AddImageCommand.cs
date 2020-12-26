using PhishingApp.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            string filename = Path.GetFileName(path);
            string extension = Path.GetExtension(path);
            string base64ImageRepresentation = Convert.ToBase64String(File.ReadAllBytes(path));
            EmailModel.Body += ("<img src =\"cid:0123456789\">\n");

            EmailModel.Body += ("Content-Type: image/" + extension.TrimStart('.') + "; name=\"" + filename + "\"\n");
            EmailModel.Body += ("Content-Disposition: inline; filename=\"" + filename + "\"\n");
            EmailModel.Body += ("Content-Transfer-Encoding: base64\n");
            EmailModel.Body += ("Content-ID: <0123456789>");
            EmailModel.Body += ("Content-Location: " + filename + "\n\n");
            EmailModel.Body += ("base64 data\n\n");
            EmailModel.Body += base64ImageRepresentation;
        }
    }
}
