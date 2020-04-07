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
	/// A CRM class manages the collection of customers. The CRM class should load the customers from file
	/// on start-up.This class should be based on a list of Customer objects.It should support operations on
	/// the Customer (e.g.add/modify/delete).
	/// 
	/// The program must make sure that data entry makes sure that customerID is unique(no
	/// duplicates allowed). You can assume that data on disk(i.e.read from files) is
	/// always valid.You may want to implement this by automatically generating a unique
	/// customerID (for example, you might want to keep track of the current highest customerID).
	/// 
	/// Author: Waldo Fouche, n9950095
	/// Date:	May 2018
	/// 
	/// </summary>
	public class CRM {
		// Defining Private Paramters.

		private List<Customer> customers;
		private string filePath = @"..\..\..\Data\customer.csv";
		public int LastAddedID;
		

		// End of Parameter Definitions

		// ---------------------------------------------------------------------

		/// <summary>
		/// If there is no CRM file at the specified location, this constructor constructors an
		/// empty CRM with no customers.Otherwise it loads the customers from file. 
		///</summary>
		public CRM(){
			customers = new List<Customer>();
			// Check for fleetFile Existance
			if (!File.Exists(filePath))
			{
				string headerTitles = "CustomerID,Title,FirstName,LastName,Gender,DOB";
				File.WriteAllText(filePath, headerTitles);
			}
		} // end of previous method - CRM.

		// ---------------------------------------------------------------------

		/// <summary>
		/// This method adds the provided customer to the customer list if the customer ID
		/// doesn’t already exist in the CRM. It returns true if the addition was successful
		/// (the customer ID wasn’t already in the CRM) and false otherwise (the customer ID
		/// was already in the CRM).
		/// </summary>
		/// <param name="customer"></param>
		/// <returns></returns>

		/// <summary>
		/// Constructs a CRM with the provided list of customers
		/// </summary>
		/// <param name="customers"></param>
		public CRM(List<Customer> customers)
		{
			this.customers = new List<Customer>(customers);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="customer"></param>
		public void AddCustomer(Customer customer)
		{
			customers.Add(customer);

			//int x = customers;
			//if (customer.CustomerID == x)
			//{
			//	return false;
			//}
			//else
			//{
			//	customers.Add(customer);
			//	return true;
			//}

		} // end of previous method - AddCusotmers (customer).

		// ---------------------------------------------------------------------

		/// <summary>
		/// This method removes the customer from the CRM if they are not currently
		/// renting a vehicle.It returns true if the removal was successful, otherwise
		/// it returns false.
		/// </summary>
		/// <param name="customer"></param>
		/// <param name="fleet"></param>
		/// <returns></returns>

		//public bool RemoveCustomer(Customer customer, Fleet fleet){

		//} // end of previous method - AddCustomers (customerID).

		// ---------------------------------------------------------------------

		/// <summary>
		/// This method removes the customer from the CRM if they are not currently renting a
		/// vehicle.It returns true if the removal was successful, otherwise it returns false.
		/// </summary>
		/// <param name="customerID"></param>
		/// <param name="fleet"></param>
		/// <returns></returns>

		//public bool RemoveCustomer(int customerID, Fleet fleet){

		//} // end of previous method - RemoveCustomers.

		// ---------------------------------------------------------------------

		/// <summary>
		/// This method returns the list of current customers.
		/// </summary>
		/// <returns></returns>

		public List<Customer> GetCustomers(){
			return customers;
		} // end of previous method - GetCustomers.

		// ---------------------------------------------------------------------

		public void RemoveCustomer(Customer customer)
		{
			customers.Remove(customer);
		}
	
		/// <summary>
		/// This method saves the current state of the CRM to file.
		/// </summary>

		public Customer GetCustomer(int CustomerID)
		{
			foreach (Customer customer in customers)
			{
				if (customer.CustomerID == CustomerID)
				{
					return customer;
				}
			}
			return null;
		}

		/// <summary>
		/// 
		/// </summary>
		public void LoadFromFile()
		{
			 if (customers.Count() >= 1) // Only headers are found in the file
			{
				// Sets Inital Customer ID to 0.
				LastAddedID = 0;
			}
			else
			{
				System.IO.StreamReader fileReader = new System.IO.StreamReader(filePath);

				// Reads and Ignores the Header Titles
				fileReader.ReadLine();

				string line;
				while ((line = fileReader.ReadLine()) != null)
				{
					string[] sn = line.Split(',');
					Customer newCustomer = new Customer(int.Parse(sn[0]), sn[1].ToString(), sn[2], sn[3],
														(Customer.GenderEnum)Enum.Parse(typeof(Customer.GenderEnum), sn[4]), sn[5]);
					customers.Add(newCustomer);

					LastAddedID = int.Parse(sn[0]);
				}
				fileReader.Close();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public void SaveToFile(){
			StreamWriter writeFile = new StreamWriter(filePath);

			// Wrtes the header text
			writeFile.WriteLine("CustomerID,Title,FirstName,LastName,Gender,DOB");

			// Writes file foreach Customer loaded into customers List
			foreach (Customer customer in customers)
			{
				writeFile.WriteLine($"{customer.CustomerID},{customer.Title},{customer.FirstNames}" +
					$",{customer.LastName},{customer.Gender.ToString()},{customer.DateOfBirth.ToString()}");
			};

			writeFile.Close();

		} // end of previous method - SaveToFle.

		// ---------------------------------------------------------------------
	}
}