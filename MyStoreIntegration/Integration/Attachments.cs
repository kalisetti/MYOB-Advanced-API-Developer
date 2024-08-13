using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyStoreIntegration.Default;
using System.IO;

namespace MyStoreIntegration.Integration {
	internal class Attachments {
		//Adding a file to a stock item record
		public static void AddFileToStockItem(DefaultSoapClient soapClient) {
			Console.WriteLine("Adding a file to a stock item record...");

			//Input data
			string inventoryID = "CONGRILL";

			//The path to the file that you need to attach to the stock item
			string filePath = "C:\\Users\\Siva\\Downloads\\";

			//The name of the file
			string fileName = "download.jpg";

			//Read the file data
			byte[] filedata;
			using (FileStream file = System.IO.File.Open(Path.Combine(filePath, fileName), FileMode.Open)) {
				filedata = new byte[file.Length];
				file.Read(filedata, 0, filedata.Length);
			}

			//Add the file to the stock item record
			StockItem stockItem = new StockItem {
				InventoryID = new StringSearch { Value = inventoryID }
			};

			Default.File[] stockItemFiles = new[] {
				new MyStoreIntegration.Default.File {
					Name = fileName,
					Content = filedata
				}
			};

			soapClient.PutFiles(stockItem, stockItemFiles);
		}

		//Adding a note to a stock item record
		public static void AddNoteToStockItem(DefaultSoapClient soapClient) {
			Console.WriteLine("Adding a note to a stock item record...");

			//Stock item data
			string inventoryID = "CONGRILL";
			string noteText = "My note";

			//Find the stock item in the system, and specify the note text
			StockItem stockItemToBeUpdated = new StockItem {
				InventoryID = new StringSearch { Value = inventoryID },
				Note = noteText
			};
			StockItem stockItem = (StockItem)soapClient.Put(stockItemToBeUpdated);

			//Display the summary of the created stock item
			Console.WriteLine("Inventory ID: " + stockItem.InventoryID.Value);
			Console.WriteLine("Note text: " + stockItem.Note);
			Console.WriteLine();
			Console.WriteLine("Press any key to continue");
			Console.ReadLine();
		}

		//Retrieving the files that are attached to a stock item
		public static void ExportStockItemFiles(DefaultSoapClient soapClient) {
			Console.WriteLine("Retrieving the files that are attached to a stock item...");

			//Parameters of filtering
			string inventoryID = "AAMACHINE1";

			//Filter the items by inventory ID
			StockItem stockItemToBeFound = new StockItem {
				InventoryID = new StringSearch { Value = inventoryID },
				ImageUrl = new StringReturn(),
				ReturnBehavior = ReturnBehavior.OnlySpecified
			};

			//Get the stock item record
			StockItem stockItem = (StockItem)soapClient.Get(stockItemToBeFound);

			//Get the files that are attached to the stock item and
			//save them on a local disc
			if (stockItem != null && stockItem.ImageUrl != null) {
				//Get the attached files
				Default.File[] files = soapClient.GetFiles(stockItem);

				//Save the files on disc
				foreach (Default.File file in files) {
					//The file name obtained from MYOB Advanced has the following
					//format: Stock Items (<Inventory ID>)\<File Name>
					string fileName = Path.GetFileName(file.Name);
					System.IO.File.WriteAllBytes(fileName, file.Content);
				}
			}
		}
	}
}
