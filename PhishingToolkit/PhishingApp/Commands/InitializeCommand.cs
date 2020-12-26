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
	public class InitializeCommand : ICommand
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


		private StatisticsModel statisticsModel;

		public StatisticsModel StatisticsModel
		{
			get { return statisticsModel; }
			set { statisticsModel = value; }
		}
		public InitializeCommand(StatisticsModel statisticsModel)
		{
			StatisticsModel = statisticsModel;
		}



		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			int numOfSentMail = 0;
			using (StreamReader sr = new StreamReader("sentMails.txt"))
			{
				numOfSentMail = Int32.Parse(sr.ReadLine());
			}

			StatisticsModel.SentMails = numOfSentMail;

			int counter = 0;
			string line = string.Empty;
			using (StreamReader sr = new StreamReader("database.txt"))
			{
				while ((line = sr.ReadLine()) != null)
				{
					counter++;
				}
			}

			StatisticsModel.FormsFilled = counter;
		}
	}
}
