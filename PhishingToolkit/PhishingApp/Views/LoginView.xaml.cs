using Contracts.DatabaseModel;
using DatabaseRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PhishingApp.Views
{
	/// <summary>
	/// Interaction logic for LoginView.xaml
	/// </summary>
	public partial class LoginView : Page
	{
		public LoginView()
		{
			InitializeComponent();
		}
		

		private void LoginBtn_Click(object sender, RoutedEventArgs e)
		{
			DataRepository dr = new DataRepository();
			List<LoginModel> Users = new List<LoginModel>();
			try
			{
				Users = dr.GetUsers();
			}
			catch (InvalidOperationException)
			{
				Console.WriteLine("Users not initialized!");
			}

			foreach (LoginModel u in Users)
			{
				if (usernameTxtBox.Text.Equals(u.Username) && passwordTxtBox.Text.Equals(u.Password))
				{
					this.Content = new PhishingApp.Views.MainView();
				}
			}

			informationLbl.Content = "Password or username incorrect, please try again";
			informationLbl.Foreground = Brushes.Red;
		}
	}
}
