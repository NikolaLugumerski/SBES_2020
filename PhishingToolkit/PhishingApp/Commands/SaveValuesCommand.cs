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
	public class SaveValuesCommand : ICommand
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
		public SaveValuesCommand(StatisticsModel statisticsModel)
		{
			StatisticsModel = statisticsModel;
		}

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			using (StreamWriter sw = new StreamWriter("sentMails.txt"))
			{
				sw.WriteLine(StatisticsModel.SentMails.ToString());
			}

		}
	}
}
