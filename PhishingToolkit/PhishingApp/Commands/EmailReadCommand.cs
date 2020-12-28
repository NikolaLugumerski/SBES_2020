using PhishingApp.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PhishingApp.Commands
{
	public class EmailReadCommand : ICommand
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

		public EmailReadCommand(EmailModel em)
		{
			EmailModel = em;
		}

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			string path = BrowseTxtFiles();
			LoadEmails(path);
		}

		public string BrowseTxtFiles()
		{
			Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

			dlg.DefaultExt = ".txt";
			dlg.Filter = "Text|*.txt|All|*.*";

			Nullable<bool> result = dlg.ShowDialog();

			if (result == true)
			{
				return dlg.FileName;
			}

			return "error";
		}

		public void LoadEmails(string path)
		{
			if (path == null)
				return;


			string emails = EmailModel.Emails;
				
			if (path.Equals("error"))
			{
				emails = "There was an error in BrowseTxtFiles";
				return;
			}

			try
			{
				string email = string.Empty;
				using (StreamReader sr = new StreamReader(path))
				{
					while ((email = sr.ReadLine()) != null)
					{
						if (IsValidEmail(email))
						{
							emails += email + "\n";
						}
						else
						{
							MessageBox.Show("Mails in .txt file are not in a valid format!");
						}
					}
				}
			}
			catch (IOException e)
			{
				Console.WriteLine("The file could not be read:");
				Console.WriteLine(e.Message);
			}

			//mozda bude morala provera da li su email-ovi svi unikatni HashSet lagano

			EmailModel.Emails = emails;
		}

		public static bool IsValidEmail(string email)
		{
			if (string.IsNullOrWhiteSpace(email))
				return false;

			try
			{
				// Normalize the domain
				email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
									  RegexOptions.None, TimeSpan.FromMilliseconds(200));

				// Examines the domain part of the email and normalizes it.
				string DomainMapper(Match match)
				{
					// Use IdnMapping class to convert Unicode domain names.
					var idn = new IdnMapping();

					// Pull out and process domain name (throws ArgumentException on invalid)
					string domainName = idn.GetAscii(match.Groups[2].Value);

					return match.Groups[1].Value + domainName;
				}
			}
			catch (RegexMatchTimeoutException e)
			{
				Console.WriteLine(e);
				return false;
			}
			catch (ArgumentException e)
			{
				Console.WriteLine(e);
				return false;
			}

			try
			{
				return Regex.IsMatch(email,
					@"^[^@\s]+@[^@\s]+\.[^@\s]+$",
					RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
			}
			catch (RegexMatchTimeoutException)
			{
				return false;
			}
		}
	}
}
