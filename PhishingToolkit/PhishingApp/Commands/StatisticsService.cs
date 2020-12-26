using Common;
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
		public void SendData(string data)
		{

			using (StreamWriter sw = new StreamWriter("database.txt", true))
			{
				sw.WriteLine(data);
			}
		}
	}
}
