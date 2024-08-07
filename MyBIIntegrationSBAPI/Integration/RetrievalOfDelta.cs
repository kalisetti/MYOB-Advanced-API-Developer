﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyBIIntegrationSBAPI.MyBIIntegration;
using System.Threading;
using System.IO;

namespace MyBIIntegrationSBAPI.Integration {
	internal class RetrievalOfDelta {
		//Exporting the list of the stock items that satisfy two conditions
		public static void ExportStockItems(Screen screen) {
			Console.WriteLine("Retrieving the list of stock items...");

			//Specify a locale to make MYOB Advanced ERP handle dates correctly
			screen.SetLocaleName(Thread.CurrentThread.CurrentCulture.ToString());

			//Get the schema of the Stock Items (IN202500) form and
			//configure the sequence of commands
			IN202500Content schema = PX.Soap.Helper.GetSchema<IN202500Content>(screen);

			//Configure the sequence of commands
			var commands = new Command[] {
				//Specify the elements whose values should be exported
				schema.StockItemSummary.ServiceCommands.EveryInventoryID,
				schema.StockItemSummary.InventoryID,
				schema.StockItemSummary.Description,
				schema.StockItemSummary.ItemStatus,
				schema.GeneralSettingsItemDefaults.ItemClass,
				//schema.GeneralSettingsUnitOfMeasureBaseUnit.BaseUnit,
				new Field {
					ObjectName = schema.StockItemSummary.InventoryID.ObjectName,
					FieldName = "LastModifiedDateTime"
				},
				schema.WarehouseDetails.Warehouse,
				schema.WarehouseDetails.QtyOnHand
			};

			//Filter the records to be exported
			var filters = new Filter[] {
				//Export only the records that have the Active status 
				new Filter {
					Field = schema.StockItemSummary.ItemStatus,
					Condition = FilterCondition.Equals,
					Value = "Active",
					Operator = FilterOperator.And
				},

				//And only the records that were modified within the last month
				new Filter {
					Field = new Field { 
						ObjectName = schema.StockItemSummary.InventoryID.ObjectName,
						FieldName = "LastModifiedDateTime"
					},
					Condition = FilterCondition.Greater,
					Value = DateTime.Now.AddDays(-1).ToLongDateString()
				}
			};

			//Export stock item records
			String[][] items = screen.IN202500Export(commands, filters, 0, true, false);

			//Save the data to a CSV file
			using (StreamWriter file = new StreamWriter("StockItems.csv")) {
				foreach (string[] rows in items) {
					foreach (string row in rows) {
						file.Write(row + ";");
					}
					file.WriteLine();
				}
				file.Close();
			};
		}
	}
}
