using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyBIIntegrationSBAPI.MyBIIntegration;

namespace MyBIIntegrationSBAPI {
	internal class Program {
		static void Main(string[] args) {
			Console.WriteLine("MyBIIntegrationSBAPI...");
			using (Screen screen = new Screen()) { 
				//Specify the connection parameters
				screen.CookieContainer = new System.Net.CookieContainer();
				screen.Url = Properties.Settings.Default.MyBIIntegrationSBAPI_MyBIIntegration_Screen;

				//Sign in to MYOB Advanced ERP
				screen.Login(
					Properties.Settings.Default.Username,
					Properties.Settings.Default.Password
				);

				try {
					//You will add the integration code here
					Console.WriteLine("After Login...");
				} catch (Exception e) {
					Console.WriteLine(e);
					Console.WriteLine();
					Console.WriteLine("Press any key to continue");
					Console.ReadLine();
				} finally {
					//Sign out from MYOB Advanced ERP
					Console.WriteLine("Logout...");
					screen.Logout();
				}
			}
			Console.ReadLine();
		}
	}
}
