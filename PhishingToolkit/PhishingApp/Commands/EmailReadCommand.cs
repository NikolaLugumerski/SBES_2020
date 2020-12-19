﻿using PhishingApp.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
						emails += email + "\n";
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
	}
}
