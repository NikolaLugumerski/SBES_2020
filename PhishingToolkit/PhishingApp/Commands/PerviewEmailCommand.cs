using PhishingApp.Model;
using PhishingApp.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Annotations;
using System.Windows.Input;

namespace PhishingApp.Commands
{
	public class PerviewEmailCommand : ICommand
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

		public PerviewEmailCommand(EmailModel emailModel)
		{
			EmailModel = emailModel;
		}


		public bool CanExecute(object parameter)
		{
			if (EmailModel.Body == null)
				return false;

			return true;
		}

		public void Execute(object parameter)
		{
			using (StreamWriter sw = new StreamWriter("emailPerview.txt"))
			{
				sw.Write(EmailModel.Body);
			}

			PerviewWindow perviewWindow = new PerviewWindow();
			perviewWindow.Show();
		}
	}
}
