using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyStoreIntegration.Default;

namespace MyStoreIntegration.Integration {
	internal class UpdateOfRecords {
		//Updating a customer record
		public static void UpdateCustomer(DefaultSoapClient soapClient) {
			Console.WriteLine("Updating a customer record...");

			//Customer data
			//Specify the email address of a customer that exists in the system
			string customerMainContactEmail = "info@jevy-comp.con";

			//Specify one of the customer classes that are configured in the system
			string customerClass = "INTL";
			string contactTitle = "Mr.";
			string contactFirstName = "Jack";
			string contactLastName = "Green";
			string contactEmail = "green@jevy-comp.con";

			//Select the needed customer record and
			//specify the values that should be updated
			Customer customerToBeUpdated = new Customer {
				ReturnBehavior = ReturnBehavior.OnlySpecified,
				CustomerID = new StringReturn(),
				MainContact = new Contact {
					//Search for the customer record by email address
					Email = new StringSearch { Value = customerMainContactEmail },
				},
				CustomerClass = new StringValue { Value = customerClass },

				//Specify the values of the customer billing contact
				BillingContactSameAsMain = new BooleanValue { Value = false },
				BillingContact = new Contact {
					Email = new StringValue { Value = contactEmail },
					Attention = new StringValue {
						Value = contactTitle + " " +
					contactFirstName + " " + contactLastName
					}
				}
			};

			//Update the customer record with the specified values
			Customer updCustomer = (Customer)soapClient.Put(customerToBeUpdated);

			//Display the ID and customer class of the updated record
			Console.WriteLine("Customer ID: " + updCustomer.CustomerID.Value);
			Console.WriteLine("Customer class: " + updCustomer.CustomerClass.Value);
			Console.WriteLine("Billing contact name: " + 
				updCustomer.BillingContact.Attention.Value);
			Console.WriteLine("Billing contact email: " +
				updCustomer.BillingContact.Email.Value);
			Console.WriteLine();
			Console.WriteLine("Press any key to continue");
			Console.ReadLine();
		}
	}
}
