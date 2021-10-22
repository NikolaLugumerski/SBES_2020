
using Contracts.DatabaseModel;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Text;

namespace Common
{
	[ServiceContract]
	public interface IFlag
	{
		[OperationContract]
		void SendData(VictimModel data);
	}
}
