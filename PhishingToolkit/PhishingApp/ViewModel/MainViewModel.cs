﻿using Common;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace PhishingApp.ViewModel
{
	public class MainViewModel : UserControl
	{
		public static readonly DependencyProperty PieChartModelProperty = DependencyProperty.Register(
			"PieChartModel",
			typeof(PieChartModel),
			typeof(MainViewModel),
			new PropertyMetadata(default(PieChartModel)));

		public PieChartModel PieChartModel
		{
			get => (PieChartModel)GetValue(MainViewModel.PieChartModelProperty);
			set => SetValue(MainViewModel.PieChartModelProperty, value);
		}


		private EmailModel emailModel;

		public EmailModel EmailModel
		{
			get { return emailModel; }
			set { emailModel = value; }
		}


		private static StatisticsModel statisticsModel;

		public static StatisticsModel StatisticsModel
		{
			get { return statisticsModel; }
			set { statisticsModel = value; }
		}

		private ValidationModel validationModelChangeLinks;

		public ValidationModel ValidationModelChangeLinks
		{
			get { return validationModelChangeLinks; }
			set { validationModelChangeLinks = value; }
		}

		private ValidationModel validationModelAddLink;

		public ValidationModel ValidationModelAddLink
		{
			get { return validationModelAddLink; }
			set { validationModelAddLink = value; }
		}

		private ValidationModel validationModelPreview;

		public ValidationModel ValidationModelPreview
		{
			get { return validationModelPreview; }
			set { validationModelPreview = value; }
		}

		private ValidationModel validationModelSendEmail;

		public ValidationModel ValidationModelSendEmail
		{
			get { return validationModelSendEmail; }
			set { validationModelSendEmail = value; }
		}

		public Func<ChartPoint, string> PointLabel { get; set; }

		public static EmailReadCommand EmailReadCommand { get; set;}
        public static SendEmailCommand SendEmailCommand { get; set; }
		public static ParseEmailCommand ParseEmailCommand { get; set; }
		public static PreviewEmailCommand PreviewEmailCommand { get; set; }
		public static ChangeLinksCommand ChangeLinksCommand { get; set; }
		public static AddImageCommand AddImageCommand { get; set; }
		public static StatisticCommand StatisticCommand { get; set; }
		public static InitializeCommand InitializeCommand { get; set; }
		public static ShowExploitedVictimsCommand ShowExploitedVictimsCommand { get; set; }
		public static AddLinkCommand AddLinkCommand { get; set; }

		DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
		
		public MainViewModel()
		{
			ValidationModelChangeLinks = new ValidationModel();
			ValidationModelAddLink = new ValidationModel();
			ValidationModelPreview = new ValidationModel();
			ValidationModelSendEmail = new ValidationModel();

			StatisticsModel = new StatisticsModel();

			InitializeCommand = new InitializeCommand(StatisticsModel);
			InitializeCommand.Execute(null);


			PieChartModel = new PieChartModel(StatisticsModel);
			PointLabel = chartPoint =>
			   string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);

		    EmailModel = new EmailModel();

			ParseEmailCommand = new ParseEmailCommand(EmailModel);
			ChangeLinksCommand = new ChangeLinksCommand(EmailModel, ValidationModelChangeLinks);
			EmailReadCommand = new EmailReadCommand(EmailModel);
			SendEmailCommand = new SendEmailCommand(EmailModel, StatisticsModel, PieChartModel, ValidationModelSendEmail);
			PreviewEmailCommand = new PreviewEmailCommand(EmailModel, ValidationModelPreview);
			AddImageCommand = new AddImageCommand(EmailModel);
			StatisticCommand = new StatisticCommand(StatisticsModel, PieChartModel);
			ShowExploitedVictimsCommand = new ShowExploitedVictimsCommand(StatisticsModel);
			AddLinkCommand = new AddLinkCommand(EmailModel, ValidationModelAddLink);

			ServiceHost svc = new ServiceHost(typeof(StatisticsService));
			svc.AddServiceEndpoint(typeof(IFlag), new NetTcpBinding(), new Uri("net.tcp://localhost:4000/IFlag"));

//			svc.Open();

			dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
			dispatcherTimer.Interval = new TimeSpan(0, 0, 10);
			dispatcherTimer.Start();

		}


		private void dispatcherTimer_Tick(object sender, EventArgs e)
		{
			StatisticCommand.Execute("tick");
			CommandManager.InvalidateRequerySuggested();
		}

		



	}
}


