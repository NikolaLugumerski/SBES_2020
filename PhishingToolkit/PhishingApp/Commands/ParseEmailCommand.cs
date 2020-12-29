using MimeKit;
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
	public class ParseEmailCommand : ICommand
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
		
		public ParseEmailCommand(EmailModel emailModel)
		{
			EmailModel = emailModel;
		}


		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			string path = BrowseEMLFiles();
			ParseEMLFile(path);
		}

		public string BrowseEMLFiles()
		{
			Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

			dlg.DefaultExt = ".eml";
			dlg.Filter = "Eml|*.eml|All|*.*";

			Nullable<bool> result = dlg.ShowDialog();

			if (result == true)
			{
				return dlg.FileName;
			}

			return "error";
		}

		public void ParseEMLFile(string path)
		{
			if (path == "error")
				return;

			var message = MimeMessage.Load(path);

			EmailModel.Body = message.HtmlBody;

			var builder = new BodyBuilder();

			builder.HtmlBody = EmailModel.Body;

			EmailModel.MessageToSend.Body = builder.ToMessageBody();


		}

	}
}
