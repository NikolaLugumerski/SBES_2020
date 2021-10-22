using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.DatabaseModel
{
	public class AppConfigModel
	{
		[Key]
		public int Id { get; set; }
		public int SentMails { get; set; }

		public AppConfigModel(int sentMails)
		{
			SentMails = sentMails;
		}

		public AppConfigModel()
		{
				
		}
	}
}
