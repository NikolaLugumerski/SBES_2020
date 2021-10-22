using DatabaseRepository;
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
			DataRepository dr = new DataRepository();
			int numOfSentMail = 0;



			try
			{
				numOfSentMail = dr.GetAppConfigs().Last().SentMails;
			}
			catch (InvalidOperationException)
			{
				numOfSentMail = 0;
			}


			StatisticsModel.SentMails = numOfSentMail;
			try
			{
				StatisticsModel.FormsFilled = dr.GetVictims().Count();
			}
			catch (InvalidOperationException)
			{
				StatisticsModel.FormsFilled = 0;
			}
		}
	}
}
