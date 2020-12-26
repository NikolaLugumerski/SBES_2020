using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
	[ServiceContract]
	public interface IFlag
	{
		void SendData(string data);
	}
}
