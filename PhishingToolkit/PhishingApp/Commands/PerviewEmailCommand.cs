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
	public class PreviewEmailCommand : ICommand
	{
		public event EventHandler CanExecuteChanged;

		private EmailModel emailModel;

		public EmailModel EmailModel
		{
			get { return emailModel; }
			set { emailModel = value; }
		}

		public PreviewEmailCommand(EmailModel emailModel)
		{
			EmailModel = emailModel;
		}


		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			using (StreamWriter sw = new StreamWriter("emailPreview.txt"))
			{
				sw.Write(EmailModel.Body);
			}

			PreviewWindow previewWindow = new PreviewWindow();
			previewWindow.Show();
		}
	}
}
