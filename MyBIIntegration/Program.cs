using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyBIIntegration.Default;
using MyBIIntegration.Integration;

namespace MyBIIntegration {
	internal class Program {
		static void Main(string[] args) {
			//Using the Default/18.20..001 endpoint
			using (Default.DefaultSoapClient soapClient = new Default.DefaultSoapClient()) {
				//Sign in to MYOB Advanced ERP
				soapClient.Login(
					Properties.Settings.Default.Username,
					Properties.Settings.Default.Password,
					Properties.Settings.Default.Tenant,
					Properties.Settings.Default.Company,
					null
				);

				try {
					//Retrieving the list of customers with contacts
					Console.WriteLine("Retrieving...");
					InitialDataRetrieval.RetrieveListOfCustomers(soapClient);
					Console.ReadLine();
				}
				catch (Exception e) {
					Console.WriteLine(e);
					Console.WriteLine();
					Console.WriteLine("Press any key to continue");
					Console.ReadLine();
				}
				finally { 
					//Sign out from MYOB Advanced ERP
					soapClient.Logout();
				}
				
			}
		}
	}
}
