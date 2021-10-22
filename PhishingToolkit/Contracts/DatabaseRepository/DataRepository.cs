
using Contracts.DatabaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseRepository
{
	public class DataRepository
	{
		public List<VictimModel> GetVictims()
		{	
			DataDBContext dataDBContext = new DataDBContext();

			return dataDBContext.Victims.ToList();
		}

		public void InsertVictim(VictimModel model)
		{
			DataDBContext dataDBContext = new DataDBContext();

			dataDBContext.Victims.Add(model);
			dataDBContext.SaveChanges();
		}


		public List<AppConfigModel> GetAppConfigs()
		{
			DataDBContext dataDBContext = new DataDBContext();

			return dataDBContext.AppConfigs.ToList();
		}

		public void InsertAppConfig(AppConfigModel model)
		{
			DataDBContext dataDBContext = new DataDBContext();
			dataDBContext.AppConfigs.Add(model);
			dataDBContext.SaveChanges();

		}

		public void InsertUser(LoginModel user)
		{
			DataDBContext dataDBContext = new DataDBContext();
			dataDBContext.Users.Add(user);
			dataDBContext.SaveChanges();

		}

		public List<LoginModel> GetUsers()
		{
			DataDBContext dataDBContext = new DataDBContext();
			
			return dataDBContext.Users.ToList();
		}

	}
}
