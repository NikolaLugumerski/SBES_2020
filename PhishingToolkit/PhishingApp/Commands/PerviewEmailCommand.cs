using MimeKit;
using PhishingApp.Model;
using PhishingApp.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Annotations;
using System.Windows.Input;

namespace PhishingApp.Commands
{
	public class PreviewEmailCommand : ICommand
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

		public PreviewEmailCommand(EmailModel emailModel)
		{
			EmailModel = emailModel;
		}


		public bool CanExecute(object parameter)
		{
			if (EmailModel.Body == null || EmailModel.SenderEmail == null || EmailModel.SenderName == null || EmailModel.RecipientName == null || EmailModel.EmailSubject == null) 
				return false;

			return true;
		}

		public void Execute(object parameter)
		{


			//using (StreamWriter sw = new StreamWriter("emailPreview.html"))
			//{
			//	sw.Write(EmailModel.Body);
			//}

			EmailModel.MessageToSend.From.Add(new MailboxAddress(EmailModel.SenderName, EmailModel.SenderEmail));
			EmailModel.MessageToSend.To.Add(new MailboxAddress(EmailModel.RecipientName, "previewmail@gmail.com"));
			EmailModel.MessageToSend.Subject = EmailModel.EmailSubject;
			EmailModel.MessageToSend.WriteTo("Preview.eml");

		
			System.Diagnostics.Process.Start("Preview.eml");
		}

	}
}
