using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MMRCManagement;

namespace TestingConsole
{
	class TestingConsole
	{
		//private string filePath = @"..\..\..\Data\fleet.csv";
		

		static void Main(string[] args)
		{
			////Console.WriteLine("Hello!");


			///// Test vehicle class ---------------------------------------------------------------------------------------

			//Vehicle addVehicle = new Vehicle("986KFG", Vehicle.VehicleClassEnum.Family, "Suzuki", "Jimny", 2009);
			//Vehicle addVehicle2 = new Vehicle("679ZHE", Vehicle.VehicleClassEnum.Luxury, "Audi", "RS3", 2017);
			//Vehicle addVehicle3 = new Vehicle("123HCB", Vehicle.VehicleClassEnum.Luxury, "Audi", "RS3", 2017);
			//Console.WriteLine(addVehicle);
			//Console.WriteLine(addVehicle2);

			///// Test Customer Class --------------------------------------------------------------------------------------

			//Customer newCustomer = new Customer(1, "Mrs", "Ella", "Truelove", Customer.GenderEnum.Female, "17/09/99");
			//Customer newCustomer2 = new Customer(2, "Mr", "James", "Adsett", Customer.GenderEnum.Male, "17/09/99");

			//Console.WriteLine(newCustomer);
			//Console.WriteLine(newCustomer2);

			////Fleet fleet1 = new Fleet();



			///// Test CSV -------------------------------------------------------------------------------------------------------

			//// Loads CSV
			//string[] fleetArray;
			////// sets up new list for read values.
			//var list = new List<string>();
			//// Opens file. 
			//var fileStream = new FileStream(@"..\..\..\Data\fleet.csv", FileMode.Open, FileAccess.Read);
			//// Writes read data to fleetArray row by row until row = null;
			//using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
			//{
			//	string line;
			//	while ((line = streamReader.ReadLine()) != null)
			//	{

			//		//string[] row = new string[];
			//		list.Add(line); // Adds Line


			//	}
			//	// Test adding Vehicles
			//	//list.Add(addVehicle3.ToString());
			//}
			////fleetArray = list.ToArray();

			//foreach (string line in list)
			//{
			//	Console.WriteLine(line);
			//}


			//foreach (string line in fleetArray)
			//{
			//	Console.WriteLine(line);
			//}


			//////display to console
			//foreach (string line in fleetArray)
			//{
			//	//Console.WriteLine(line);
			//	string[] split = line.Split(',');

			//	string x = split[1];

			//	// Test if Rego is already added
			//	if (x == addVehicle3.VehicleRego)
			//	{
			//		Console.WriteLine(x + " < - Error! This Vehicle is already added here!");

			//	}
			//	else
			//	{
			//		if (x == "TRUE")
			//		{
			//			Console.WriteLine("GPS Installed!");
			//		}
			//		else
			//		{
			//			Console.WriteLine(x);
			//		}

			//	}

			//}

			//CRM x = new CRM();

			//foreach(var line in x.GetCustomers().ToString())
			//{
			//	Console.WriteLine(line.ToString());
			//}

			// addint to fleetArray

			//foreach (string line in fleetArray)
			//{
			//	for (int i = 0; i <fleetArray.Length; i++)
			//	{
			//		string[] row = line[i];
			//	}
			//}




			//var x = new CRM();
			//var xy = x.GetCustomers();
			//Customer xf = new Customer(1, "Mrs", "Ella", "Truelove", Customer.GenderEnum.Female, "17/09/99");
			//xy.Add(xf);
			//foreach (var line in xy.ToString())
			//{
			//	Console.WriteLine(line);
			//};

			//test creating customer - check if creates new files and adds in customer
			CRM y = new CRM();
			Customer newCustomer = new Customer(9, "Mrs", "Ella", "Truelove", Customer.GenderEnum.Female, "17/09/99");
			y.AddCustomer(newCustomer);

			//test creating fleet - check if creates new files
			Fleet x = new Fleet();
			// test adding vehicles
			Vehicle newVehicle1 = new Vehicle("123HCB", Vehicle.VehicleClassEnum.Economy, "Mazda", "3", 2000, 4, Vehicle.TransmissionTypeEnum.Automatic, Vehicle.FuelTypeEnum.Petrol, false, false, 50, "Red");
			//Vehicle newVehicle2 = new Vehicle("897HOI", Vehicle.VehicleClassEnum.Family,   "Mitsubishi",  "ASX", 2010, 4,   Vehicle.TransmissionTypeEnum.Manuel,Vehicle.FuelTypeEnum.Petrol, true,false,80, "Red");
			//Vehicle newVehicle5 = new Vehicle("986KFG", Vehicle.VehicleClassEnum.Family, "Suzuki", "Jimny", 2009);
			//x.AddVehicle(newVehicle1);
			x.AddVehicle(newVehicle1);
			//x.RemoveVehicle(newVehicle1);

			y.SaveToFile();
			//x.SaveToFile();


			//foreach (char line in x.ToString())
			//{
			//	Console.WriteLine(x);
			//}

			//Console.ReadLine();
		}
	}

}
