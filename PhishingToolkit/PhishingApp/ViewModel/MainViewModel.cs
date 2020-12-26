using Common;
using PhishingApp.Commands;
using PhishingApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Configuration;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace PhishingApp.ViewModel
{
	public class MainViewModel
	{

		private EmailModel emailModel;

		public EmailModel EmailModel
		{
			get { return emailModel; }
			set { emailModel = value; }
		}


		private StatisticsModel statisticsModel;

		public StatisticsModel StatisticsModel
		{
			get { return statisticsModel; }
			set { statisticsModel = value; }
		}


		public static EmailReadCommand EmailReadCommand { get; set;}
        public static SendEmailCommand SendEmailCommand { get; set; }
		public static ParseEmailCommand ParseEmailCommand { get; set; }
		public static PreviewEmailCommand PreviewEmailCommand { get; set; }
		public static ChangeLinksCommand ChangeLinksCommand { get; set; }
		public static AddImageCommand AddImageCommand { get; set; }
		public static StatisticCommand StatisticCommand { get; set; }

		
		DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
		
		public MainViewModel()
		{
			StatisticsModel = new StatisticsModel();

			EmailModel = new EmailModel();
			ParseEmailCommand = new ParseEmailCommand(EmailModel);
			ChangeLinksCommand = new ChangeLinksCommand(EmailModel);
			EmailReadCommand = new EmailReadCommand(EmailModel);
			SendEmailCommand = new SendEmailCommand(EmailModel, StatisticsModel);
			PreviewEmailCommand = new PreviewEmailCommand(EmailModel);
			AddImageCommand = new AddImageCommand(EmailModel);
			StatisticCommand = new StatisticCommand(StatisticsModel);

			ServiceHost svc = new ServiceHost(typeof(StatisticsService));
			svc.AddServiceEndpoint(typeof(IFlag), new NetTcpBinding(), new Uri("net.tcp://localhost:4000/IFlag"));
			svc.Open();

			dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
			dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
			dispatcherTimer.Start();
		}


		private void dispatcherTimer_Tick(object sender, EventArgs e)
		{
			StatisticCommand.Execute("tick");
			ChangeLinksCommand.CanExecute("tick"); // mozda resi onaj problem 
			// Forcing the CommandManager to raise the RequerySuggested event
			CommandManager.InvalidateRequerySuggested();
		}

	}
}


