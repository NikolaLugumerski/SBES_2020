﻿using PhishingApp.Commands;
using PhishingApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

		public static EmailReadCommand EmailReadCommand { get; set;}
        public static SendEmailCommand SendEmailCommand { get; set; }
		public static ParseEmailCommand ParseEmailCommand { get; set; }

		public static PerviewEmailCommand PerviewEmailCommand { get; set; }

        public MainViewModel()
		{
			EmailModel = new EmailModel();
			ParseEmailCommand = new ParseEmailCommand(EmailModel);
			EmailReadCommand = new EmailReadCommand(EmailModel);
			SendEmailCommand = new SendEmailCommand(EmailModel);
			PerviewEmailCommand = new PerviewEmailCommand(EmailModel);

		}


	}
}


