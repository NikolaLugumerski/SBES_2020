using Contracts.DatabaseModel;
using DatabaseRepository;
using LiveCharts;
using PhishingApp.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Shapes;

namespace PhishingApp.Commands
{
	public class StatisticCommand : ICommand
	{
		private StatisticsModel	 statisticsModel;

		public StatisticsModel StatisticsModel
		{
			get { return statisticsModel; }
			set { statisticsModel = value; }
		}

		private PieChartModel pieChartModel;

		public PieChartModel PieChartModel
		{
			get { return pieChartModel; }
			set { pieChartModel = value; }
		}



		public StatisticCommand(StatisticsModel statisticsModel, PieChartModel pcm)
		{
			StatisticsModel = statisticsModel;
			PieChartModel = pcm;
		}


		
		public event EventHandler CanExecuteChanged
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}

		protected void RaiseCanExecuteChanged()
		{
			CommandManager.InvalidateRequerySuggested();
		}

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			DataRepository dr = new DataRepository();

			try
			{

				StatisticsModel.ExploitedVictims = dr.GetVictims().ToDictionary(v => v.Id.ToString());
				StatisticsModel.FormsFilled = StatisticsModel.ExploitedVictims.Count();
			}
			catch (InvalidOperationException)
			{
				StatisticsModel.FormsFilled = 0;
			}

			PieChartModel.FormsFilledSeries = new ChartValues<int>() { StatisticsModel.FormsFilled };

			try
			{
				StatisticsModel.SentMails = dr.GetAppConfigs().Last().SentMails;
			}
			catch (InvalidOperationException)
			{
				StatisticsModel.SentMails = 0;
			}

			PieChartModel.SentMailsSeries = new ChartValues<int>() { StatisticsModel.SentMails };
		}
	}
}
