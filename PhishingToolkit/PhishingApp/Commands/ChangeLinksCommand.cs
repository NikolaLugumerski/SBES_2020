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

		public ChangeLinksCommand(EmailModel em)
		{
			EmailModel = em;
		}


		public bool CanExecute(object parameter)
		{
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
