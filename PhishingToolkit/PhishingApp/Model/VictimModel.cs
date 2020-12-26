using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhishingApp.Model
{
	public class VictimModel
	{

		public string Email { get; set; }
		public string Password { get; set; }
		public DateTime Date { get; set; }

		public VictimModel(string email, string password, DateTime date)
		{
			Email = email;
			Password = password;
			Date = date;
		}
	}
}
