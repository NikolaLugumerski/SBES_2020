using MimeKit;
using PhishingApp.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Xml;

namespace PhishingApp.Commands
{
	public	class ChangeLinksCommand : ICommand
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

		private ValidationModel validationModelChangeLinks;

		public ValidationModel ValidationModelChangeLinks
		{
			get { return validationModelChangeLinks; }
			set { validationModelChangeLinks = value; }
		}



		public ChangeLinksCommand(EmailModel em, ValidationModel vm)
		{
			EmailModel = em;
			ValidationModelChangeLinks = vm;
		}


		public bool CanExecute(object parameter)
		{
			ValidationModelChangeLinks.Text = string.Empty;
			string temp = "";
			if (EmailModel.MaliciousLink == string.Empty)
			{
				temp += "Malicious link is empty" + "\n";
			}
			if (EmailModel.Body == null)
			{
				temp += "Email body is empty";
			}

			ValidationModelChangeLinks.Text = temp;

			if (EmailModel.MaliciousLink == string.Empty || EmailModel.Body == null)
				return false;

			return true;
		}

		public void Execute(object parameter)
		{
			if (!EmailModel.MaliciousLink.Contains("http://") && !EmailModel.MaliciousLink.Contains("https://"))
			{
				EmailModel.MaliciousLink = "http://" + EmailModel.MaliciousLink;
			}

			HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
			htmlDoc.LoadHtml(EmailModel.Body);
			foreach(var node in htmlDoc.DocumentNode.SelectNodes("//a"))
            {
				node.SetAttributeValue("href", EmailModel.MaliciousLink);
            }
			var changedHtml = htmlDoc.DocumentNode.WriteTo();
			EmailModel.Body = changedHtml;

			MessageBox.Show("Changed links to: " + EmailModel.MaliciousLink);
		}

	}
}
