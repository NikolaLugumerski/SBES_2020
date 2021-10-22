using Common;
using Contracts.DatabaseModel;
using DatabaseRepository;
using PhishingApp.Model;
using PhishingApp.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhishingApp.Commands
{
	public class StatisticsService : IFlag
	{

		public void SendData(VictimModel data)
		{
			DataRepository dr = new DataRepository();

			try
			{
				dr.InsertVictim(data);
			}
			catch (InvalidOperationException)
			{
				Console.WriteLine("Error during insertVictim");
			}
		}
	}
}
