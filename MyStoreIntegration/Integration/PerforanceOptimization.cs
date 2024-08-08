using MyStoreIntegration.Default;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStoreIntegration.Integration {
	internal class PerforanceOptimization {
		//Retrieving the list of sales orders of a customer
		public static void ExportSalesOrders(DefaultSoapClient soapClient) {
			Console.WriteLine("Getting the list of sales orders of a customer...");

			//Customer data
			string customerID = "C000000003";

			//Specify the customerID of a customer whose sales orders should be
			//exported and the fields to be returned
			SalesOrder ordersToBeFound = new SalesOrder {
				//Return only the specified values
				ReturnBehavior = ReturnBehavior.OnlySpecified,

				//Specify the customer whose sales order should be returned
				CustomerID = new StringSearch { Value = customerID },

				//Specify the fields of the entity and details to be returned
				OrderType = new StringReturn(),
				OrderNbr = new StringReturn(),
				CustomerOrder = new StringReturn(),
				Date = new DateTimeReturn(),
				OrderedQty = new DecimalReturn(),
				OrderTotal = new DecimalReturn(),
				Details = new SalesOrderDetail[] {
					new SalesOrderDetail {
						InventoryID = new StringReturn(),
						OrderQty = new DecimalReturn(),
						UnitPrice = new DecimalReturn(),
					}
				}
			};

			//Get the list of sales orders with details
			Entity[] soList = soapClient.GetList(ordersToBeFound);

			//Save the results to a CSV file
			using (StreamWriter file = new StreamWriter(string.Format(@"SalesOrderDetails_Customer_{0}.csv", customerID))) {
				//Add headers to the file
				file.WriteLine("OrderType;OrderNbr;CustomerID;CustomerOrder;Date;" +
					"OrderedQty;OrderTotal;InventoryID;OrderQty;UnitPrice;");

				//Write the values for each sales order
				foreach (SalesOrder salesOrder in soList) { 
					foreach (SalesOrderDetail detail in salesOrder.Details) {
						file.WriteLine(string.Format(
							"{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};",
							//Document summary
							salesOrder.OrderType.Value,
							salesOrder.OrderNbr.Value,
							salesOrder.CustomerID.Value,
							salesOrder.CustomerOrder.Value,
							salesOrder.Date.Value,
							salesOrder.OrderedQty.Value,
							salesOrder.OrderTotal.Value,
							detail.InventoryID.Value,
							detail.OpenQty.Value,
							detail.UnitCost.Value));
					}
				}
			}

			//Specify the fields in shipments to be returned
			ordersToBeFound = new SalesOrder {
				ReturnBehavior = ReturnBehavior.OnlySpecified,

				CustomerID = new StringSearch { Value = customerID },

				OrderType = new StringReturn(),
				OrderNbr = new StringReturn(),
				Shipments = new SalesOrderShipment[] {
					new SalesOrderShipment {
						ShipmentNbr = new StringReturn(),
						InvoiceNbr = new StringReturn(),
					}
				}
			};

			//Get the list of sales orders of the customer
			soList = soapClient.GetList(ordersToBeFound);

			//Save results to a CSV file
			using (StreamWriter file = new StreamWriter(string.Format(@"SalesOrderShipments_Customer_{0}.csv", customerID))) {
				//Add headers to the file
				file.WriteLine("OrderType;OrderNbr;ShipmentNbr;InvoiceNbr;");

				//Write the values for each sales order
				foreach (SalesOrder salesOrder in soList) {
					foreach (SalesOrderShipment shipment in salesOrder.Shipments) {
						file.WriteLine(string.Format("{0};{1};{2};{3};",
							//Document summary
							salesOrder.OrderType.Value,
							salesOrder.OrderNbr.Value,
							shipment.ShipmentNbr.Value,
							shipment.InvoiceNbr.Value
						));
					}
				}
			};
		}
	}
}
