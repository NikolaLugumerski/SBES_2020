using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.DatabaseModel
{
	[DataContract]
	public class VictimModel
	{
		[Key]
		public int Id { get; set; }

		[DataMember]
		public string Email { get; set; }
		
		[DataMember]
		public string Password { get; set; }
		
		[DataMember]
		public DateTime Date { get; set; }

		public VictimModel(string email, string password, DateTime date)
		{
			Email = email;
			Password = password;
			Date = date;
		}

		public VictimModel()
		{
				
		}
	}
}
