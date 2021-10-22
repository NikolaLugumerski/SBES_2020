using Contracts.DatabaseModel;

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseRepository
{
	public class DataDBContext : DbContext
	{	
		public DbSet<VictimModel> Victims { get; set; }
		public DbSet<AppConfigModel> AppConfigs { get; set; }
		public DbSet<LoginModel> Users { get; set; }
	}
}
