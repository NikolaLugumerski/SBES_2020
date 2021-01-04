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


		private ValidationModel validationModelPreview;

		public ValidationModel ValidationModelPreview
		{
			get { return validationModelPreview; }
			set { validationModelPreview = value; }
		}

		public PreviewEmailCommand(EmailModel emailModel, ValidationModel vm)
		{
			EmailModel = emailModel;
			ValidationModelPreview = vm;
		}



		public bool CanExecute(object parameter)
		{
			ValidationModelPreview.Text = string.Empty;
			string temp = "";
			if (EmailModel.Body == null)
			{
				temp += "Email body is empty";
			}

			ValidationModelPreview.Text = temp;


			if (EmailModel.Body == null) 
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

				EmailModel.BodyBuilder.TextBody = EmailModel.Body;

				if (EmailModel.Body != null)
				{
					if (EmailModel.HtmlBody == null)
					{
						EmailModel.HtmlBody = "\n" + "<p style=\"white-space: pre-line\">" + EmailModel.Body + "</p>" + "\n";
						EmailModel.HtmlBodyHelper = EmailModel.Body;

						// za preview
						EmailModel.HtmlForPreview = "\n" + "<p style=\"white-space: pre-line\">" + EmailModel.Body + "</p>" + "\n";
					}
					else
					{
						string temp = EmailModel.Body.Substring(EmailModel.HtmlBodyHelper.Length);

						if (temp == "")
						{

						}
						else
						{
							EmailModel.HtmlBody += "\n" + "<p  style=\"white-space: pre-line\">" + temp + " </p>" + "\n";

							EmailModel.HtmlBodyHelper = EmailModel.Body;

							// za preview
							EmailModel.HtmlForPreview += "\n" + "<p  style=\"white-space: pre-line\">" + temp + " </p>" + "\n";
						}
					}
				}

				// zato sto prikazujes html u body mora ova linija koda
				EmailModel.HtmlBodyHelper = EmailModel.HtmlBody;


				EmailModel.BodyBuilder.HtmlBody = EmailModel.HtmlBody;
				EmailModel.Body = EmailModel.BodyBuilder.HtmlBody;


			

				using (StreamWriter sw = new StreamWriter("Preview.html"))
				{
					sw.Write(EmailModel.HtmlForPreview);
				}

				System.Diagnostics.Process.Start("Preview.html");

			}
		}

	}
}
