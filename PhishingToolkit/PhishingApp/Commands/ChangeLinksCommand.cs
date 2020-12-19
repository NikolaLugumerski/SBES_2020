using MimeKit;
using PhishingApp.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Xml;

namespace PhishingApp.Commands
{
	public	class ChangeLinksCommand : ICommand
	{
		private EmailModel emailModel;

		public event EventHandler CanExecuteChanged;

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
			return true;
		}

		public void Execute(object parameter)
		{
			string html = EmailModel.Body;
			/*
			StringBuilder newHtml = new StringBuilder(html);
			Regex r = new Regex(@"\<a href=\""([^\""]+)\"">"); // 1st capture for the replacement and 2nd for the find
			foreach (var match in r.Matches(html).Cast<Match>().OrderByDescending(m => m.Index))
			{
				string text = EmailModel.MaliciousLink;
				string newHref = DBTranslate(text);
				newHtml.Remove(match.Groups[0].Index, match.Groups[0].Length);
				newHtml.Insert(match.Groups[0].Index, newHref);
			}
			EmailModel.Body = newHtml.ToString();
			*/
			HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
			htmlDoc.LoadHtml(EmailModel.Body);
			var htmlNodes = htmlDoc.DocumentNode.SelectNodes("//a");
			for(int i=0; i<htmlNodes.Count; i++)
            {
				htmlNodes[i].Attributes["href"].Value = EmailModel.MaliciousLink;
				
            }
			//treba vratiti ove htmlNodes u htmlDoc
			EmailModel.Body = htmlDoc.Text;
		}

		static string DBTranslate(string s)
		{
			return "junk_" + s;
		}

	}
}
