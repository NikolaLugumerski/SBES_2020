using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.DatabaseModel
{
	public class LoginModel
	{
		[Key]
		public int Id { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }

		public LoginModel( string username, string password)
		{
			Username = username;
			Password = password;
		}

		public LoginModel()
		{

		}
	}
}
