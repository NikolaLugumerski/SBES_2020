using LiveCharts;
using LiveCharts.Wpf.Charts.Base;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhishingApp.Model
{
	public class PieChartModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;


		private void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}



		private ChartValues<int> sentMailsSeries;

		public ChartValues<int> SentMailsSeries
		{
			get { return sentMailsSeries; }
			set
			{ 
				sentMailsSeries = value;
				OnPropertyChanged("SentMailsSeries");
			}

		}

		private ChartValues<int> formsFilledSeries;

		public ChartValues<int> FormsFilledSeries
		{
			get { return formsFilledSeries; }
			set 
			{ 
				formsFilledSeries = value;
				OnPropertyChanged("FormsFilledSeries");
			}
		}



		public PieChartModel(StatisticsModel statisticsModel)
		{
			FormsFilledSeries = new ChartValues<int>() { statisticsModel.FormsFilled };
			SentMailsSeries = new ChartValues<int>() { statisticsModel.SentMails };
		}
	}
}
