using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMRCManagement
{
	/// <summary>
	/// 
	/// A Fleet class manages the rental of vehicles. The fleet class should load the vehicle fleet from file on
	/// start-up.This class should be based on list of Vehicle objects.It should support operations on the
	/// fleet, such as adding, modifying and deleting vehicles.It should also handle the renting and returning
	/// of vehicles.
	/// 
	/// Author: Waldo Fouche, n9950095
	/// Date:	May 2018
	/// 
	/// </summary>

	public class Fleet
	{
		// Properties ------------------------------------------------------------------------------------------

		private List<Vehicle> vehicles;
		
		public Dictionary<string, int> rentals;
		private Dictionary<string, int> rego;
		private string fleetFile = @"..\..\..\Data\fleet.csv";
		private string rentalFile = @"..\..\..\Data\rentals.csv";

		// Methods ---------------------------------------------------------------------------------------------

		/// <summary>
		/// 
		/// </summary>

		public Fleet()
		{
			vehicles = new List<Vehicle>();
			rego = new Dictionary<string, int>();
			rentals = new Dictionary<string, int>();
			//rentedList = new List<string>();
			// Check for fleetFile Existance

			if (!File.Exists(fleetFile))
			{ // If File does not exist, creates file and adds in headers
				string headerTitles = "Rego,Make,Model,Year,VehicleClass,NumSeats,Transmission,EngineSize, Turbo, Fuel"
					+ ",GPS,SunRoof,Colour,DailyRate";
				File.WriteAllText(fleetFile, headerTitles);
			}

			if (!File.Exists(rentalFile))
			{ // If File does not exist, creates file and adds in headers
				string headerTitles = "Vehicle,Customer";
				File.WriteAllText(rentalFile, headerTitles);
			}
		}// end of previous method - Fleet()

		// ------------------------------------------------------------------------------------------------------

		/// <summary>
		/// 
		/// </summary>
		/// <param name="vehicle"></param>
		public void AddVehicle(Vehicle vehicle)
		{
			vehicles.Add(vehicle);
		}// end of previous method - AddVehicle() 

		// ------------------------------------------------------------------------------------------------------

		/// <summary>
		/// 
		/// </summary>
		/// <param name="vehicle"></param>

		public void RemoveVehicle(Vehicle vehicle)
		{
			vehicles.Remove(vehicle);
		}// end of previous method - AddVehicle() 

		// ------------------------------------------------------------------------------------------------------

		/// <summary>
		/// 
		/// </summary>
		/// <param name="rego"></param>
		/// <returns></returns>

		public bool RemoveVehicle(string rego)
		{
			return true;
		}// end of previous method - AddVehicle() 

		// ------------------------------------------------------------------------------------------------------

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>

		public List<Vehicle> GetFleet()
		{
			return vehicles;
		} // end of previous method - GetFleet() 

		// ------------------------------------------------------------------------------------------------------

		/// <summary>
		/// 
		/// </summary>
		/// <param name="VehicleRego"></param>
		/// <returns></returns>

		public Vehicle GetVehicle(string VehicleRego)
		{
			foreach (Vehicle vehicle in vehicles)
			{
				if (vehicle.VehicleRego == VehicleRego)
				{
					return vehicle;
				}
			}
			return null;
		}

		// ------------------------------------------------------------------------------------------------------

		/// <summary>
		/// 
		/// </summary>
		/// <param name="query"></param>
		/// <returns></returns>

		public List<Vehicle> GetVehicles(string query)
		{
			List<Vehicle> requestedVehicle = new List<Vehicle>();
			string[] queryComponents = query.Split(' ');

			// Foreach Query Keyword
			for (int i = 0; i < queryComponents.Length; i = +2)
			{
				if (i == 0)
				{
					foreach (Vehicle vehicleNotRented in GetFleet(false))
					{
						if (vehicleNotRented.GetAttributeList().Contains(queryComponents[i]))
						{
							requestedVehicle.Add(vehicleNotRented);
						}
					}
				}
				else if ((queryComponents[i - 1] == "OR"))
				{
					List<Vehicle> tempVehicle = new List<Vehicle>();

					foreach (Vehicle vehicleNotRented in GetFleet(false))
					{
						if (vehicleNotRented.GetAttributeList().Contains(queryComponents[i]))
						{
							tempVehicle.Add(vehicleNotRented);
						}
					}

					requestedVehicle = requestedVehicle.Union(tempVehicle).ToList<Vehicle>();
				}
				else if ((queryComponents[i - 1] == "AND"))
				{
					List<Vehicle> tempVehicle = new List<Vehicle>();

					foreach (Vehicle vehicleNotRented in GetFleet(false))
					{
						if (vehicleNotRented.GetAttributeList().Contains(queryComponents[i]))
						{
							tempVehicle.Add(vehicleNotRented);
						}
					}
					requestedVehicle = requestedVehicle.Union(tempVehicle).ToList<Vehicle>();
				}
			}
			return requestedVehicle;
		} // end of previous method - GetVehicles(string query)

		// ------------------------------------------------------------------------------------------------------

		/// <summary>
		/// 
		/// </summary>
		/// <param name="rented"></param>
		/// <returns></returns>

		public List<Vehicle> GetFleet(bool rented)
		{
			List<Vehicle> requestedVehicle = new List<Vehicle>();
			if (rented == true)
			{
				foreach (Vehicle vehicle in vehicles)
				{
					string rego = vehicle.VehicleRego;

					if (rentals.ContainsKey(rego))
					{
						requestedVehicle.Add(vehicle);
					}
				}
			}
			else if (rented == false)
			{
				foreach (Vehicle vehicle in vehicles)
				{
					requestedVehicle.Add(vehicle);
				}
			}
			//foreach (Vehicle vehicle in vehicles)
			//{
			//	string rego = vehicle.VehicleRego;
			//	CRM x = new CRM();
			//	var c = x.GetCustomers();
				
			//	foreach (Customer customer in c)
			//	{
			//		int cID = customer.CustomerID;
			//		if (!rentals.ContainsValue(cID) == rented)
			//		{
			//			requestedVehicle.Add(vehicle);
			//		}
			//	}
			//}

			return requestedVehicle;
		} // end of previous method - GetFleet() 

		// ----------------------------------------------------------------------------------------------------

		/// <summary>
		/// 
		/// </summary>
		/// <param name="vehicleRego"></param>
		/// <returns></returns>

		public bool IsRented(string vehicleRego)
		{
			if (rentals.ContainsKey(vehicleRego))
			{
				return true;
			}
			else
			{
				return false;
			}
		} // end of previous method - IsRented() 

		// ----------------------------------------------------------------------------------------------------

		/// <summary>
		/// 
		/// </summary>
		/// <param name="customerID"></param>
		/// <returns></returns>

		public bool IsRenting(int customerID)
		{
			if (rentals.ContainsValue(customerID))
			{
				return true;
			}
			else
			{
				return false;
			}

		}

		// ------------------------------------------------------------------------------------------------------

		/// <summary>
		/// 
		/// </summary>

		public void SaveToFile()
		{
			StreamWriter writeFile = new StreamWriter(fleetFile);

			// Wrtes the header text
			writeFile.WriteLine("Rego,Make,Model,Year,VehicleClass,NumSeats,Transmission,Fuel"
					+ ",GPS,SunRoof,Colour,DailyRate");

			// Writes file foreach Customer loaded into customers List
			foreach (Vehicle vehicle in vehicles)
			{
				writeFile.WriteLine($"{vehicle.VehicleRego},{vehicle.Make},{vehicle.Model},{vehicle.Year}," +
					$"{vehicle.VehicleClass},{vehicle.NumSeats},{vehicle.TransmissionType},{vehicle.EngineSize},{vehicle.Turbo}," +
					$"{vehicle.FuelType},{vehicle.GPS},{vehicle.SunRoof},{vehicle.Colour},{vehicle.DailyRate}");
			};

			writeFile.Close();

		} // end of previous method - SaveToFle().


		// ------------------------------------------------------------------------------------------------------

		/// <summary>
		/// 
		/// </summary>

		public void LoadFromFile()
		{
			StreamReader readFile = new StreamReader(fleetFile);

			readFile.ReadLine();
			string line;
			while ((line = readFile.ReadLine()) != null)
			{
				string[] sn = line.Split(',');
				Vehicle newVehicle = new Vehicle(sn[0], sn[1], sn[2], int.Parse(sn[3]), (Vehicle.VehicleClassEnum)
					Enum.Parse(typeof(Vehicle.VehicleClassEnum), sn[4]), int.Parse(sn[5]),
					(Vehicle.TransmissionTypeEnum)Enum.Parse(typeof(Vehicle.TransmissionTypeEnum), sn[6]), int.Parse(sn[7]), bool.Parse(sn[8]),
					(Vehicle.FuelTypeEnum)Enum.Parse(typeof(Vehicle.FuelTypeEnum), sn[9]), bool.Parse(sn[10]),
					bool.Parse(sn[11]), sn[12], double.Parse(sn[13]));

				vehicles.Add(newVehicle);
			}
			readFile.Close();
		} // end of previous method - LoadFromFile().

		public void LoadFromFileRented()
		{

			StreamReader readFile = new StreamReader(rentalFile);

			readFile.ReadLine();
			string line;
			while ((line = readFile.ReadLine()) != null)
			{
				string[] sn = line.Split(',');
				

				rentals.Add(sn[0], int.Parse(sn[1]));
			}
			readFile.Close();

		}
	}
}