using PhishingApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Media3D;

namespace PhishingApp.Commands
{
	public class AddDivCommand : ICommand
	{

        private EmailModel emailModel;

        public EmailModel EmailModel
        {
            get { return emailModel; }
            set { emailModel = value; }
        }

        public AddDivCommand(EmailModel emailModel)
        {
            EmailModel = emailModel;
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
            EmailModel.Body = EmailModel.Body + "\r\n";

            if (EmailModel.HtmlBody == null)
            {
                EmailModel.HtmlBody = "\n" + "<p>" + EmailModel.Body + "</p>" + "\n";
                EmailModel.HtmlBodyHelper = EmailModel.Body;
            }
            else
            {
                string temp = EmailModel.HtmlBody.Substring(EmailModel.HtmlBodyHelper.Length);

                EmailModel.HtmlBody += "\n" + "<p>" + temp + " </p>" + "\n";

                EmailModel.HtmlBodyHelper = EmailModel.Body;
            }


        }
    }
}
