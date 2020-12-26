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

			int counter = 0;
			string line = string.Empty;
			using (StreamReader sr = new StreamReader("database.txt"))
			{
				while ((line = sr.ReadLine()) != null)
				{
					string[] tokens = line.Split(';');

					StatisticsModel.ExploitedVictims[tokens[0]] = new VictimModel(tokens[0], tokens[1], DateTime.Parse(tokens[2]));

					counter++;
				}
			}

			StatisticsModel.FormsFilled = counter;
			PieChartModel.FormsFilledSeries = new ChartValues<int>() { counter };
		}
	}
}
