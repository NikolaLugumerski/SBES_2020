using PhishingApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PhishingApp.Commands
{
	public class ImportEmailCommand : ICommand
	{
		private EmailModel emailModel;

		public EmailModel EmailModel
		{
			get { return emailModel; }
			set { emailModel = value; }
		}

		public ImportEmailCommand(EmailModel emailModel)
		{
			EmailModel = emailModel;
		}

		public event EventHandler CanExecuteChanged;

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			throw new NotImplementedException();
		}
	}
}
