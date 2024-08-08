using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyStoreIntegrationSBAPI.SO643000;
using MyStoreIntegrationSBAPI.Integration;

namespace MyStoreIntegrationSBAPI {
	internal class Program {
		static void Main(string[] args) {
			using (Screen screen = new Screen()) {
				//Specify the connection parameters
				screen.CookieContainer = new System.Net.CookieContainer();
				screen.Url = Properties.Settings.Default.MyStoreIntegrationSBAPI_SO643000_Screen;

				//Sign in to MYOB Advanced
				screen.Login(
					"admin",
					"Admin@123456789"
				);

				try {
					Reports.GetPrintableInvoice(screen);
				}
				catch (Exception e) {
					Console.WriteLine(e);
					Console.WriteLine();
					Console.WriteLine("Press any key to continue");
					Console.WriteLine();
				}
				finally { 
					//Sign out from MYOB Advanced
					screen.Logout();
				}
			}
		}
	}
}
