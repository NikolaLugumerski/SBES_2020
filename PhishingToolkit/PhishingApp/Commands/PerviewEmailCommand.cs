using MailKit;
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
			if (EmailModel.Body == null || EmailModel.SenderName.Equals("") || EmailModel.RecipientName.Equals("") || EmailModel.EmailSubject.Equals("")) 
				return false;

			return true;
		}

		public void Execute(object parameter)
		{

			if (EmailModel.HtmlImported)
			{
				using (StreamWriter sw = new StreamWriter("PreviewHtml.html")) 
				{
					sw.Write(EmailModel.Body);
				}


				System.Diagnostics.Process.Start("PreviewHtml.html");
			}
			else
			{
				EmailModel.MessageToSend.From.Add(new MailboxAddress(EmailModel.SenderName, EmailModel.SenderEmail));
				EmailModel.MessageToSend.To.Add(new MailboxAddress(EmailModel.RecipientName, "previewmail@gmail.com"));
				EmailModel.MessageToSend.Subject = EmailModel.EmailSubject;

				EmailModel.BodyBuilder.TextBody = EmailModel.Body;

				if (EmailModel.Body != null)
				{
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
				}

				// zato sto prikazujes html u body mora ova linija koda
				EmailModel.HtmlBodyHelper = EmailModel.HtmlBody;


				EmailModel.BodyBuilder.HtmlBody = EmailModel.HtmlBody;
				EmailModel.Body = EmailModel.BodyBuilder.HtmlBody;


				EmailModel.MessageToSend.Body = EmailModel.BodyBuilder.ToMessageBody();

				EmailModel.MessageToSend.WriteTo("Preview.eml");

				System.Diagnostics.Process.Start("Preview.eml");

			}
		}

	}
}
