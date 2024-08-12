using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyStoreIntegration.Default;

namespace MyStoreIntegration.Integration {
	internal class CreationOfRecords {
		//Creating a shipment
		public static void CreateShipment(DefaultSoapClient soapClient) {
			Console.WriteLine("Creating a shipment...");

			//Shipment data
			string shipmentType = "Shipment";
			string customerID = "C000000008";
			string warehouse = "MAIN";

			//Sales order with the Open status for the specified customer
			string firstOrderNbr = "000004";
			string firstOrderType = "SO";

			//Sales order with the Open status for the specified customer
			string secondOrderNbr = "000006";
			string secondOrderType = "SO";

			//Find the first sales order to be shipped
			SalesOrder orderToBeFound1 = new SalesOrder {
				ReturnBehavior = ReturnBehavior.OnlySpecified,
				OrderType = new StringSearch { Value = firstOrderType },
				OrderNbr = new StringSearch { Value = firstOrderNbr },
				Details = new SalesOrderDetail[] {
					new SalesOrderDetail {
						ReturnBehavior= ReturnBehavior.OnlySpecified,
						InventoryID = new StringReturn(),
						WarehouseID = new StringReturn()
					}
				}
			};
			SalesOrder orderForShipment1 = (SalesOrder)soapClient.Get(orderToBeFound1);

			//Find the second sales order to be shipped
			SalesOrder orderToBeFound2 = new SalesOrder {
				ReturnBehavior = ReturnBehavior.OnlySpecified,
				OrderType = new StringSearch { Value = secondOrderType },
				OrderNbr = new StringSearch { Value = secondOrderNbr },
				Details = new SalesOrderDetail[] {
					new SalesOrderDetail {
						ReturnBehavior= ReturnBehavior.OnlySpecified,
						InventoryID = new StringReturn(),
						WarehouseID = new StringReturn()
					}
				}
			};
			SalesOrder orderForShipment2 = (SalesOrder)soapClient.Get(orderToBeFound2);

			//Select all stock items from the sales orders for shipment
			List<ShipmentDetail> shipmentDetails = new List<ShipmentDetail>();
			foreach (SalesOrderDetail item in orderForShipment1.Details) {
				shipmentDetails.Add(new ShipmentDetail {
					OrderType = orderForShipment1.OrderType,
					OrderNbr = orderForShipment1.OrderNbr,
					InventoryID = item.InventoryID,
					WarehouseID = item.WarehouseID
				});
			}

			foreach (SalesOrderDetail item in orderForShipment2.Details) {
				shipmentDetails.Add(new ShipmentDetail {
					OrderType = orderForShipment2.OrderType,
					OrderNbr = orderForShipment2.OrderNbr,
					InventoryID = item.InventoryID,
					WarehouseID = item.WarehouseID
				});
			}

			//Specify the values of the new shipment
			Shipment shipment = new Shipment {
				ReturnBehavior = ReturnBehavior.OnlySpecified,
				Type = new StringValue { Value = shipmentType },
				ShipmentNbr = new StringReturn(),
				Status = new StringReturn(),
				CustomerID = new StringValue { Value = customerID },
				WarehouseID = new StringValue { Value = warehouse },
				Details = shipmentDetails.ToArray()
			};

			//Create a shipment with the specified values
			Shipment newShipment = (Shipment)soapClient.Put(shipment);

			//Display the summary of the created record
			Console.WriteLine("Shipment number: " + newShipment.ShipmentNbr.Value);
			Console.WriteLine("Shipment type: " + newShipment.Type.Value);
			Console.WriteLine("Shipment status: " + newShipment.Status.Value);
			Console.WriteLine();
			Console.WriteLine("Press any key to continue");
			Console.ReadLine();
		}
	}
}
