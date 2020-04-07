using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using MMRCManagement;

namespace MMRC{
	/// <summary>
	/// 
	/// This script creates the main windows form, contains all of the methods for each element, aswell as containing the
	/// main functionality of the program.
	/// 
	/// Author: Waldo Fouche, n9950095
	/// Date:	May 2018
	/// 
	/// </summary>

	public partial class Form1 : Form
	{


		/// ------------------------------------------------------------------------------------------------------
		/// ----------------------------------- Properties & Definitions -----------------------------------------
		/// ------------------------------------------------------------------------------------------------------


		// Initilizes Fleet fleet.
		Fleet fleet = new Fleet();
		// Intilizes CRM Customers
		CRM customers = new CRM();

		// Creates Selected variables
		private Customer selectedCustomer = null;
		private Vehicle selectedVehicle = null;
		private CRM selectedRental = null;
		private List<string> rentedList;



		// dataGridViewHeaders
		private string[] fleetColumns = new string[] { "Rego", "Make", "Model", "Year", "VehicleClass", "NumSeats",
			"Transmission", "Fuel", "GPS", "SunRoof", "Colour", "DailyRate" };
		private string[] customerColumns = new string[] { "ID", "First name", "Last name", "DOB" };

		// ------------------------------------------------------------------------------------------------------

		/// <summary>
		/// 
		/// </summary>

		public Form1()
		{
			InitializeComponent();
			SetupCRM();
			SetupFleet();
			SetUpComboBox();
			//fleet.rentals.Add("123XYZ", 0); // testing code for search
			SetUpRentalReport();
			populateDataGridViewRentalReport();
		}// end of previous method - PopulateDataGridViewFleet()

		// ------------------------------------------------------------------------------------------------------

		private void SetUpRentalReport()
		{
			rentedList = new List<string>();
			// Call ToList.
			List<KeyValuePair<string, int>> list = fleet.rentals.ToList();

			// Loop over list.
			List<string> tempList = new List<string>();
			foreach (KeyValuePair<string, int> pair in list)
			{
				string p = pair.ToString();
				// removes [] from p
				string xp = p.TrimStart('[');
				string xps = xp.TrimEnd(']');
				tempList.Add(xps);
				foreach (Vehicle line in fleet.GetFleet())
				{
					if (p.Contains(line.VehicleRego))
					{
						string rental = $"{xps},{line.DailyRate}";
						rentedList.Add(rental);
					}
				}
			}
			
		}
		/// <summary>
		/// 
		/// </summary>

		public void SetUpComboBox()
		{
			// Modify Vehicles
			comboBoxModifyVehicleClass.DataSource = Enum.GetValues(typeof(Vehicle.VehicleClassEnum));
			comboBoxModiyTransmission.DataSource = Enum.GetValues(typeof(Vehicle.TransmissionTypeEnum));
			comboBoxModifyFuelType.DataSource = Enum.GetValues(typeof(Vehicle.FuelTypeEnum));

			// Add Vehicles
			comboBoxAddVehicleClass.DataSource = Enum.GetValues(typeof(Vehicle.VehicleClassEnum));
			comboBoxAddVehicleTransmission.DataSource = Enum.GetValues(typeof(Vehicle.TransmissionTypeEnum));
			comboBoxAddVehicleFuelType.DataSource = Enum.GetValues(typeof(Vehicle.FuelTypeEnum));

			// Add Customers
			comboBoxModifyGender.DataSource = Enum.GetValues(typeof(Customer.GenderEnum));
			comboBoxAddCustomerGender.DataSource = Enum.GetValues(typeof(Customer.GenderEnum));

			// Search Vehicle Select Customer
			comboBoxCustomer.DataSource = customers.GetCustomers();

		}// end of previous method - PopulateDataGridViewFleet()

		/// ------------------------------------------------------------------------------------------------------
		/// --------------------------------------- Vehciles Tab -------------------------------------------------
		/// ------------------------------------------------------------------------------------------------------

		/// <summary>
		/// Sets Up the fleet for use
		/// </summary>

		public void SetupFleet()
		{
			fleet.LoadFromFile();
			fleet.LoadFromFileRented();
			PopulateDataGridViewFleet();
		}// end of previous method - SetupFleet()

		// ------------------------------------------------------------------------------------------------------

		/// <summary>
		/// populates data grid view with fleet 
		/// </summary>

		private void PopulateDataGridViewFleet()
		{
			dataGridViewFleet.Rows.Clear();

			var fleetList = fleet.GetFleet();

			if (fleetList.Count() > 0)
			{
				foreach (Vehicle vehicle in fleet.GetFleet())
				{
					dataGridViewFleet.Rows.Add(new string[] { vehicle.VehicleRego, vehicle.Make, vehicle.Model,
						vehicle.Year.ToString(), vehicle.VehicleClass.ToString(),vehicle.NumSeats.ToString(),
						vehicle.TransmissionType.ToString(),vehicle.EngineSize.ToString(),vehicle.Turbo.ToString(), vehicle.FuelType.ToString(), vehicle.GPS.ToString(),
						vehicle.SunRoof.ToString(), vehicle.Colour, vehicle.DailyRate.ToString()});
				}
			}
		}// end of previous method - PopulateDataGridViewFleet()

		// ------------------------------------------------------------------------------------------------------

		/// <summary>
		/// grabs information from seleccted row and puts it into the text boxes to modify
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>

		private void dataGridViewFleet_SelectionChanged(object sender, EventArgs e)
		{
			buttonModifyVehicle.Enabled = true;
			int rowsCount = dataGridViewFleet.SelectedRows.Count;

			if (rowsCount == 0 || rowsCount > 1)
			{
				selectedVehicle = null;
			}
			else // Sets ModifygroupBox Inputs to selected vehicle Params
			{
				string selectedRego = (dataGridViewFleet.SelectedRows[0].Cells[0].Value.ToString());
				selectedVehicle = fleet.GetVehicle(selectedRego);
				textBoxModifyRego.Text = selectedVehicle.VehicleRego;
				textBoxModifyMake.Text = selectedVehicle.Make;
				textBoxModifyModel.Text = selectedVehicle.Model;
				comboBoxModifyVehicleClass.SelectedIndex = (int)selectedVehicle.VehicleClass;
				textBoxModifyYear.Text = selectedVehicle.Year.ToString();
				comboBoxModiyTransmission.SelectedIndex = (int)selectedVehicle.TransmissionType;
				numericUpDownModifyEngineSize.Value = (int)selectedVehicle.EngineSize;
				checkBoxModifyTurbo.Checked = selectedVehicle.Turbo;
				comboBoxModifyFuelType.SelectedIndex = (int)selectedVehicle.FuelType;
				numericUpDownModifyNumSeats.Value = selectedVehicle.NumSeats;
				checkBoxModifyGPS.Checked = selectedVehicle.GPS;
				checkBoxModifySunroof.Checked = selectedVehicle.SunRoof;
				textBoxModifyColour.Text = selectedVehicle.Colour;
				numericUpDownModifyDailyRate.Value = int.Parse(selectedVehicle.DailyRate.ToString());
			}
		}// end of previous method - dataGridViewFleet_SelectionChanged()

		// ------------------------------------------------------------------------------------------------------

		// TextBox input validation

		private void textBoxAddVehicleYear_Leave(object sender, EventArgs e)
		{
			int f;
			if (int.TryParse(textBoxAddVehicleYear.Text, out f) == false)
			{
				textBoxAddVehicleYear.Text = null;
				MessageBox.Show("Invalid input! - Must be 4 numbers", "Vehicle Year");
				pictureBox8.Visible = true;
			}
			else
			{
				pictureBox8.Visible = false;
			}

		}
		private void textBoxAddVehicleRego_Leave(object sender, EventArgs e)
		{
			Regex pattern = new Regex(@"^[0-9]{3}[A-Z]{3}$");
			if (pattern.IsMatch(textBoxAddVehicleRego.Text))
			{
				// Check if matches Existing Rego
				var vehicles = fleet.GetFleet();

				foreach (Vehicle vehicle in vehicles)
				{
					if (vehicle.VehicleRego == textBoxAddVehicleRego.Text)
					{
						//pictureBox5.Visible = true;
						textBoxAddVehicleRego.Text = null;
						MessageBox.Show("Invalid input! - The Rego is already in use!", "Vehicle Registration");

					}
					else
					{
						pictureBox5.Visible = false;
					}
				}

			}
			else
			{
				textBoxAddVehicleRego.Text = null;
				MessageBox.Show("Invalid input! - Must be 3 numbers followed by 3 capital letters", "Vehicle Registration");
			}
		}

		private void textBoxAddVehicleMake_Leave(object sender, EventArgs e)
		{
			if (!string.IsNullOrWhiteSpace(textBoxAddVehicleMake.Text))
			{
				pictureBox6.Visible = false;
			}
		}

		private void textBoxAddVehicleModel_Leave(object sender, EventArgs e)
		{
			if (!string.IsNullOrWhiteSpace(textBoxAddVehicleModel.Text))
			{
				pictureBox7.Visible = false;
			}

		}

		private void comboBoxAddVehicleClass_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (comboBoxAddVehicleClass.SelectedIndex >= 1)
			{
				pictureBox9.Visible = false;
			}

		}
		private void buttonModifyVehicle_Click(object sender, EventArgs e)
		{
			groupBoxAddVehicle.Enabled = false;
			groupBoxAddVehicle.Visible = false;

			// Unhides and Enables Modify Fleet Group Box.
			groupBoxModifyFleet.Enabled = true;
			groupBoxModifyFleet.Visible = true;

			textBoxModifyRego.Enabled = false;
			groupBoxModifyFleetOptions.Enabled = false;


		}// end of previous method - dataGridViewFleet_SelectionChanged()

		// ------------------------------------------------------------------------------------------------------

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>

		private void buttonModifyFleetSubmit_Click(object sender, EventArgs e)
		{
			// Removes Current Instance of Vehicle.
			fleet.RemoveVehicle(selectedVehicle);

			string rego = textBoxModifyRego.Text;
			string make = textBoxModifyMake.Text;
			string model = textBoxModifyModel.Text;
			Vehicle.VehicleClassEnum vehicleClass = (Vehicle.VehicleClassEnum)comboBoxModifyVehicleClass.SelectedValue;
			int year = int.Parse(textBoxModifyYear.Text);
			Vehicle.TransmissionTypeEnum transmissionType = (Vehicle.TransmissionTypeEnum)comboBoxModiyTransmission.SelectedValue;
			int engineSize = int.Parse(numericUpDownAddVehicleEngineSize.Value.ToString());
			bool turbo = checkBoxModifyTurbo.Checked;
			Vehicle.FuelTypeEnum fuelType = (Vehicle.FuelTypeEnum)comboBoxModifyFuelType.SelectedValue;
			int numSeats = int.Parse(numericUpDownModifyNumSeats.Value.ToString());
			bool GPS = checkBoxModifyGPS.Checked;
			bool Sunroof = checkBoxModifySunroof.Checked;
			string colour = textBoxModifyColour.Text;
			double dailyRate = double.Parse(numericUpDownModifyDailyRate.Value.ToString());

			Vehicle newVehicle = new Vehicle(rego, make, model, year, vehicleClass, numSeats, transmissionType, engineSize, turbo, fuelType,
				GPS, Sunroof, colour, dailyRate);
			fleet.AddVehicle(newVehicle);
			PopulateDataGridViewFleet();

			groupBoxModifyFleet.Visible = false;
			groupBoxModifyFleetOptions.Enabled = true;

		}// end of previous method - dataGridViewFleet_SelectionChanged()

		// ------------------------------------------------------------------------------------------------------

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>

		private void buttonModifyFleetCancel_Click(object sender, EventArgs e)
		{
			// Hides and Disables Modify Fleet Group Box.
			groupBoxModifyFleet.Enabled = false;
			groupBoxModifyFleet.Visible = false;

			textBoxModifyRego.Text = "";
			textBoxModifyMake.Text = "";
			textBoxModifyModel.Text = "";
			comboBoxModifyVehicleClass.SelectedIndex = -1;


			// Enable Modify fleet Options groupbox
			groupBoxModifyFleetOptions.Enabled = true;
		}// end of previous method - dataGridViewFleet_SelectionChanged()

		// ------------------------------------------------------------------------------------------------------

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>		

		private void buttonRemoveVehicle_Click(object sender, EventArgs e)
		{
			string rego = selectedVehicle.VehicleRego.ToString();
			DialogResult removeVehicle = MessageBox.Show($"Are you sure you want to delete Vehicle {rego}?", "Confirm Vehicle Removal",
			MessageBoxButtons.YesNo);
			if (removeVehicle == DialogResult.Yes)
			{
				fleet.RemoveVehicle(selectedVehicle);
				PopulateDataGridViewFleet();
			}

		}// end of previous method - dataGridViewFleet_SelectionChanged()

		// ------------------------------------------------------------------------------------------------------


		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>

		private void buttonAddVehicle_Click(object sender, EventArgs e)
		{
			groupBoxModifyFleet.Enabled = false;
			groupBoxModifyFleet.Visible = false;

			//Unhides and Enables Add Vehicle Group Box.
			groupBoxAddVehicle.Enabled = true;
			groupBoxAddVehicle.Visible = true;

			// Disable Fleet Modify, Add Delete groupBox
			groupBoxModifyFleetOptions.Enabled = false;

		}// end of previous method - dataGridViewFleet_SelectionChanged()

		// ------------------------------------------------------------------------------------------------------

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>

		private void buttonAddVehicleSubmit_Click(object sender, EventArgs e)
		{
			// Validate Required Inputs

			// Validate Rego
			if (string.IsNullOrWhiteSpace(textBoxAddVehicleRego.Text))
			{
				MessageBox.Show("Error! - The Vehicle Rego is required!");
				return;
			}

			// Validate Make
			if (string.IsNullOrWhiteSpace(textBoxAddVehicleMake.Text))
			{
				MessageBox.Show("Error! - The Vehicle Make is required!");
				return;
			}

			// Validate Model
			if (string.IsNullOrWhiteSpace(textBoxAddVehicleModel.Text))
			{
				MessageBox.Show("Error! - The Vehicle Model is required!");
				return;
			}

			// Validate Class
			if (string.IsNullOrWhiteSpace(comboBoxAddVehicleClass.SelectedValue.ToString()))
			{
				MessageBox.Show("Error! - The Vehicle Class is required!");
				return;
			}
			// Validate Year
			if (string.IsNullOrWhiteSpace(textBoxAddVehicleYear.Text))
			{
				MessageBox.Show("Error! - The Vehicle Year is required!");
				return;
			}
			string rego = textBoxAddVehicleRego.Text;
			string make = textBoxAddVehicleMake.Text;
			string model = textBoxAddVehicleModel.Text;
			Vehicle.VehicleClassEnum vehicleClass = (Vehicle.VehicleClassEnum)comboBoxAddVehicleClass.SelectedValue;
			int year = int.Parse(textBoxAddVehicleYear.Text);
			Vehicle.TransmissionTypeEnum transmissionType = (Vehicle.TransmissionTypeEnum)comboBoxAddVehicleTransmission.SelectedValue;
			int engineSize = int.Parse(numericUpDownAddVehicleEngineSize.Value.ToString());
			bool turbo = checkBoxAddVehicleTurbo.Checked;
			Vehicle.FuelTypeEnum fuelType = (Vehicle.FuelTypeEnum)comboBoxAddVehicleFuelType.SelectedValue;
			int numSeats = int.Parse(numericUpDownAddVehicleNumSeats.Value.ToString());
			bool GPS = checkBoxAddVehicleGPS.Checked;
			bool Sunroof = checkBoxAddVehicleSunRoof.Checked;
			string colour;
			if (textBoxAddVehicleColour.Text == "")
			{
				colour = "Black";
			}
			else
			{
				colour = textBoxAddVehicleColour.Text;
			}

			double dailyRate = double.Parse(numericUpDownAddVehicleDailyRate.Value.ToString());

			Vehicle newVehicle = new Vehicle(rego, make, model, year, vehicleClass, numSeats, transmissionType,
				engineSize, turbo, fuelType, GPS, Sunroof, colour, dailyRate);
			fleet.AddVehicle(newVehicle);
			PopulateDataGridViewFleet();

			// Resets the Inputs to empty/ Defualts
			textBoxAddVehicleRego.Text = "";
			textBoxAddVehicleMake.Text = "";
			textBoxAddVehicleModel.Text = "";
			textBoxAddVehicleYear.Text = "";
			numericUpDownAddVehicleNumSeats.Value = 2;
			numericUpDownAddVehicleEngineSize.Value = 4;
			checkBoxAddVehicleTurbo.Checked = false;
			checkBoxAddVehicleGPS.Checked = false;
			checkBoxAddVehicleSunRoof.Checked = false;
			textBoxAddVehicleColour.Text = "";
			numericUpDownAddVehicleDailyRate.Value = 50;

			groupBoxAddVehicle.Enabled = false;
			groupBoxAddVehicle.Visible = false;

			groupBoxModifyFleetOptions.Enabled = true;

		}// end of previous method - dataGridViewFleet_SelectionChanged()

		// ------------------------------------------------------------------------------------------------------

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>

		private void buttonAddVehicleCancel_Click_1(object sender, EventArgs e)
		{

			groupBoxAddVehicle.Enabled = false;
			groupBoxAddVehicle.Visible = false;

			// Enable Modify fleet Options groupbox
			groupBoxModifyFleetOptions.Enabled = true;

		}// end of previous method - dataGridViewFleet_SelectionChanged()

		// ------------------------------------------------------------------------------------------------------

		private void buttonAddVehicleCancel_Click(object sender, EventArgs e)
		{
			// Hides and Disables Add Vehicle Group Box.
			groupBoxModifyFleet.Enabled = false;
			groupBoxModifyFleet.Visible = false;

			// Enable Fleet Modify, Add Delete groupBox
			groupBoxModifyFleetOptions.Enabled = true;
		}// end of previous method - dataGridViewFleet_SelectionChanged()


		/// ------------------------------------------------------------------------------------------------------
		/// --------------------------------------- Customers Tab ------------------------------------------------
		/// ------------------------------------------------------------------------------------------------------

		/// <summary>
		/// 
		/// </summary>

		public void SetupCRM()
		{
			customers.LoadFromFile();
			PopulateDataGridViewCustomers();
		}// end of previous method - dataGridViewFleet_SelectionChanged()

		// ------------------------------------------------------------------------------------------------------

		/// <summary>
		/// 
		/// </summary>

		private void PopulateDataGridViewCustomers()
		{
			dataGridViewCustomers.Rows.Clear();

			var customerList = customers.GetCustomers();

			// Chekcs if List is Empty before execution

			if (customerList.Count() > 0)
			{
				foreach (Customer customer in customers.GetCustomers())
				{
					dataGridViewCustomers.Rows.Add(new string[] {customer.CustomerID.ToString(),customer.Title,
						customer.FirstNames,customer.LastName,customer.Gender.ToString(),customer.DateOfBirth });
				}
			}

		}// end of previous method - dataGridViewFleet_SelectionChanged()

		// ------------------------------------------------------------------------------------------------------

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>

		private void dataGridViewCustomers_SelectionChanged(object sender, EventArgs e)
		{
			numericUpDownModifyCustomerID.Enabled = false;
			//numericUpDownModifyCustomerID.Value = -1;
			buttonRemoveCRM.Enabled = true;
			int rowsCount = dataGridViewCustomers.SelectedRows.Count;

			if (rowsCount == 0 || rowsCount > 1)
			{
				selectedCustomer = null;
			}
			else
			{
				int selectedId = int.Parse(dataGridViewCustomers.SelectedRows[0].Cells[0].Value.ToString());
				selectedCustomer = customers.GetCustomer(selectedId);

				// Pulls Data From Selected Customer
				numericUpDownModifyCustomerID.Value = selectedCustomer.CustomerID;
				textBoxModifyTitle.Text = selectedCustomer.Title;
				textBoxModifyFirstName.Text = selectedCustomer.FirstNames;
				textBoxModifyLastName.Text = selectedCustomer.LastName;
				comboBoxModifyGender.SelectedIndex = (int)selectedCustomer.Gender;
				textBoxModifyDOB.Text = selectedCustomer.DateOfBirth.ToString();
			}
		}// end of previous method - dataGridViewFleet_SelectionChanged()

		// ------------------------------------------------------------------------------------------------------

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>

		private void buttonModifyCRM_Click(object sender, EventArgs e)
		{
			groupBoxAddCustomer.Enabled = false;
			groupBoxAddCustomer.Visible = false;


			groupBoxModifyCustomers.Enabled = true;
			groupBoxModifyCustomers.Visible = true;

		}// end of previous method - dataGridViewFleet_SelectionChanged()

		// ------------------------------------------------------------------------------------------------------

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void textBoxAddCustomerDOB_Leave(object sender, EventArgs e)
		{
			//Regex pattern = new Regex(@"^[0-9]{2}[./-][0-9]{2}[./-][0-9]{4}$");
			//if (!pattern.IsMatch(textBoxAddVehicleRego.Text))
			//{
			//	MessageBox.Show("Error! - Invalid DOB Format. Must be in DD/MM/YYYY", "Customer Date of Birth");
			//}

		} // end of previous method - dataGridViewFleet_SelectionChanged()

		// ------------------------------------------------------------------------------------------------------

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void buttonModifyCustomerSubmit_Click(object sender, EventArgs e)
		{
			int cID = int.Parse(numericUpDownModifyCustomerID.Value.ToString());
			string title = textBoxModifyTitle.Text;
			string firstName = textBoxModifyFirstName.Text;
			string lastName = textBoxModifyLastName.Text;
			Customer.GenderEnum gender = (Customer.GenderEnum)comboBoxModifyGender.SelectedValue;
			string DOB = textBoxModifyDOB.Text;

			Customer newCustomer = new Customer(cID, title, firstName, lastName, gender, DOB);
			customers.RemoveCustomer(selectedCustomer);
			customers.AddCustomer(newCustomer);

			PopulateDataGridViewCustomers();
		}// end of previous method - dataGridViewFleet_SelectionChanged()

		// ------------------------------------------------------------------------------------------------------

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>

		private void buttonModifyCustomerCancel_Click(object sender, EventArgs e)
		{
			groupBoxModifyCustomers.Enabled = false;
			groupBoxModifyCustomers.Visible = false;

		}// end of previous method - dataGridViewFleet_SelectionChanged()

		// ------------------------------------------------------------------------------------------------------

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>

		private void buttonRemoveCRM_Click(object sender, EventArgs e)
		{

			groupBoxAddCustomer.Visible = false;
			groupBoxAddCustomer.Enabled = false;
			groupBoxModifyCustomers.Visible = false;
			groupBoxAddCustomer.Enabled = false;

			int customerID = selectedCustomer.CustomerID;

			string customerName = selectedCustomer.FirstNames;
			string customerLastName = selectedCustomer.LastName;

			DialogResult removeCustomer = MessageBox.Show($"Are you sure you want to delete Customer {customerID} - {customerName} {customerLastName}?", "Confirm Vehicle Removal",
			MessageBoxButtons.YesNo);
			if (removeCustomer == DialogResult.Yes)
			{
				customers.RemoveCustomer(selectedCustomer);
				PopulateDataGridViewCustomers();
			}

		}// end of previous method - dataGridViewFleet_SelectionChanged()

		// ------------------------------------------------------------------------------------------------------

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>

		private void buttonAddCRM_Click(object sender, EventArgs e)
		{
			// Disables Modfiy customers if open.

			groupBoxModifyCustomers.Visible = false;
			groupBoxModifyCustomers.Enabled = false;
			// Sets Customer ID to lastAddedCustomer + 1.


			// Sets NumericUpDown to value.
			numericUpDownAddCustomerID.Enabled = false; // Disabled so the value cannot be changed;

			var CustomerList = customers.GetCustomers();
			if (CustomerList.Count() > 0)
			{
				// Check ID is unique and only + 1.
				int LastAddedID = customers.LastAddedID;
				int nextID = LastAddedID + 1; // Set Initial Value to 0

				if (LastAddedID == nextID - 1)
				{
					numericUpDownAddCustomerID.Value = nextID;
					customers.LastAddedID = nextID;
				}
				else
				{
					nextID = LastAddedID + 1;
				}
			}
			groupBoxAddCustomer.Visible = true;
			groupBoxAddCustomer.Enabled = true;

		}// end of previous method - dataGridViewFleet_SelectionChanged()

		// ------------------------------------------------------------------------------------------------------

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>

		private void buttonAddCustomerSubmit_Click(object sender, EventArgs e)
		{
			// Validate Required Inputs

			// Validate Title
			if (string.IsNullOrWhiteSpace(textBoxAddCustomerTitle.Text))
			{
				MessageBox.Show("Error! - Title is required!");
				return;
			}
			// Validate FirstName
			if (string.IsNullOrWhiteSpace(textBoxAddCustomerFirstName.Text))
			{
				MessageBox.Show("Error! - First Name is required!");
				return;
			}

			// Validate LastName
			if (string.IsNullOrWhiteSpace(textBoxAddCustomerLastName.Text))
			{
				MessageBox.Show("Error! - Last Name is required!");
				return;
			}

			if (string.IsNullOrWhiteSpace(textBoxAddCustomerDOB.Text))
			{
				MessageBox.Show("Error! - Date Of Birth (DOB) is required!");
				return;
			}

			int cID = int.Parse(numericUpDownAddCustomerID.Value.ToString());
			string title = textBoxAddCustomerTitle.Text;
			string firstName = textBoxAddCustomerFirstName.Text;
			string lastName = textBoxAddCustomerLastName.Text;
			Customer.GenderEnum gender = (Customer.GenderEnum)comboBoxAddCustomerGender.SelectedValue;
			string DOB = textBoxAddCustomerDOB.Text;

			Customer newCustomer = new Customer(cID, title, firstName, lastName, gender, DOB);
			customers.AddCustomer(newCustomer);

			PopulateDataGridViewCustomers();

			// Check ID is unique and only + 1.
			int LastAddedID = customers.LastAddedID;
			int nextID = LastAddedID + 1;

			if (LastAddedID == nextID - 1)
			{

				numericUpDownAddCustomerID.Value = nextID;
				customers.LastAddedID = nextID;
			}

			// Resets Values in Inputs

			textBoxAddCustomerTitle.Text = null;
			textBoxAddCustomerFirstName.Text = null;
			textBoxAddCustomerLastName.Text = null;
			textBoxAddCustomerDOB.Text = null;

		}// end of previous method - dataGridViewFleet_SelectionChanged()

		// ------------------------------------------------------------------------------------------------------

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>

		private void buttonAdddCustomerCancel_Click(object sender, EventArgs e)
		{
			// if no new customer is added, the new ID is reset to what it was before Add was clicked.
			customers.LastAddedID = customers.LastAddedID - 1;
			groupBoxAddCustomer.Enabled = false;
			groupBoxAddCustomer.Visible = false;
		}// end of previous method - dataGridViewFleet_SelectionChanged()

		/// ------------------------------------------------------------------------------------------------------
		/// ------------------------------------  Rental Report Tab ----------------------------------------------
		/// ------------------------------------------------------------------------------------------------------

		// ADD SHIT HERE

		// List to contain rental report
		private List<string> rentalreport = new List<string>();

		private void populateDataGridViewRentalReport()
		{
			dataGridViewRental.Rows.Clear();

			var rentalList = fleet.GetFleet(true);

			if (rentalList.Count() > 0)
			{
				//foreach (Vehicle vehicle in fleet.GetFleet(true))
				//{
					foreach (string line in rentedList)
					{
						string[] sn = line.Split(',');
						dataGridViewRental.Rows.Add(sn[0],sn[1],sn[2]);
					}
					
				//}
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>

		private void dataGridViewRental_SelectionChanged(object sender, EventArgs e)
		{
			int rowsCount = dataGridViewRental.SelectedRows.Count;

			if (rowsCount == 0 || rowsCount > 1)
			{
				selectedRental = null;
			}
			else
			{
				//string selectedrego = dataGridViewCustomers.SelectedRows[0].Cells[0].Value.ToString();
				//selectedRental = fleet.IsRented()
				//int selectedID = int.Parse(dataGridViewCustomers.SelectedRows[0].Cells[1].Value.ToString());
				//string key = "$[selectedrego, selectedID)]";
				//fleet.rentals.Remove(key);
				//populateDataGridViewRentalReport();


			}
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void buttonReturnVehicle_Click(object sender, EventArgs e)
		{
			// remove selected customer & Vehicle from rental dictionary
			// Delete from rental list
			// reload list
		}


		/// ------------------------------------------------------------------------------------------------------
		/// ------------------------------------  Vehicle Search Tab ---------------------------------------------
		/// ------------------------------------------------------------------------------------------------------


		// Searches Based on Query input into textBoxQueryIn.
		private void buttonSearchQuery_Click(object sender, EventArgs e)
		{
			groupBoxSearchResults.Enabled = true;
			groupBoxSearchResults.Visible = true;
			dataGridViewSearchResults.Enabled = true;
			dataGridViewSearchResults.Visible = true;

			dataGridViewSearchResults.Rows.Clear();
			string input = textBoxQueryIn.Text;

			double min = double.Parse(numericUpDownMinCost.Value.ToString());
			double max = double.Parse(numericUpDownMaxCost.Value.ToString());
			foreach (Vehicle vehicle in fleet.GetFleet())
			{
				if (min < vehicle.DailyRate && vehicle.DailyRate <= max)
				{
					dataGridViewSearchResults.Rows.Add(new string[] { vehicle.VehicleRego, vehicle.Make, vehicle.Model,
					vehicle.Year.ToString(), vehicle.VehicleClass.ToString(),vehicle.NumSeats.ToString(),vehicle.TransmissionType.ToString(),
					vehicle.FuelType.ToString(), vehicle.GPS.ToString(),vehicle.SunRoof.ToString(), vehicle.Colour, vehicle.DailyRate.ToString()});
				}
			}

			//string input = textBoxQueryIn.Text;
			//foreach (Vehicle vehicle in fleet.GetVehicles(input))
			//{
			//	dataGridViewSearchResults.Rows.Add(new string[] { vehicle.VehicleRego, vehicle.Make, vehicle.Model,
			//		vehicle.Year.ToString(), vehicle.VehicleClass.ToString(),vehicle.NumSeats.ToString(),vehicle.TransmissionType.ToString(),
			//		vehicle.FuelType.ToString(), vehicle.GPS.ToString(),vehicle.SunRoof.ToString(), vehicle.Colour, vehicle.DailyRate.ToString()});
			//}
		}

		private void buttonShowAll_Click(object sender, EventArgs e)
		{
			groupBoxSearchResults.Enabled = true;
			groupBoxSearchResults.Visible = true;
			dataGridViewSearchResults.Enabled = true;
			dataGridViewSearchResults.Visible = true;

			groupBoxCreateRental.Enabled = true;

			dataGridViewSearchResults.Rows.Clear();

			foreach (Vehicle vehicle in fleet.GetFleet())
			{
				dataGridViewSearchResults.Rows.Add(new string[] { vehicle.VehicleRego, vehicle.Make, vehicle.Model, vehicle.Year.ToString(), vehicle.VehicleClass.ToString(),
					vehicle.NumSeats.ToString(),vehicle.TransmissionType.ToString(), vehicle.FuelType.ToString(), vehicle.GPS.ToString(),
					vehicle.SunRoof.ToString(), vehicle.Colour, vehicle.DailyRate.ToString()});
			}

		}



		private void topPanel_Paint(object sender, PaintEventArgs e)
		{

		}
		private void textBoxQueryIn_TextChanged(object sender, EventArgs e)
		{

		}

		private void dataGridViewSearchResults_SelectionChanged(object sender, EventArgs e)
		{
			groupBoxCreateRental.Enabled = true;
			int rowsCount = dataGridViewSearchResults.SelectedRows.Count;

			if (rowsCount == 0 || rowsCount > 1)
			{
				selectedVehicle = null;
			}
			else // Sets ModifygroupBox Inputs to selected vehicle Params
			{
				string selectedRego = (dataGridViewSearchResults.SelectedRows[0].Cells[0].Value.ToString());
				selectedVehicle = fleet.GetVehicle(selectedRego);

			}
		}

		/// ------------------------------------------------------------------------------------------------------
		/// ------------------------------------ On Exit Execute  ------------------------------------------------
		/// ------------------------------------------------------------------------------------------------------
		private void saveRentalToFile()
		{
			StreamWriter writeFile = new StreamWriter(@"..\..\..\Data\rentals.csv");

			// Wrtes the header text
			writeFile.WriteLine("Vehicle,Customer");

			// Writes file foreach Customer loaded into customers List
			foreach (string line in rentedList)
			{
				string[] sn = line.Split(',');
				// removes dailyrate
				string addToFile = $"{sn[0]}, {sn[1]}";
				writeFile.WriteLine(addToFile);
			};

			writeFile.Close();
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			customers.SaveToFile();
			fleet.SaveToFile();
			saveRentalToFile();

		}

		private void buttonRentToCustomer_Click(object sender, EventArgs e)
		{
			dataGridViewRental.Rows.Clear();
			dataGridViewRental.Rows.Add(new string[] { selectedVehicle.VehicleRego, comboBoxCustomer.SelectedIndex.ToString(), selectedVehicle.DailyRate.ToString() });
		}

		private void numericUpDown1_Enter(object sender, EventArgs e)
		{
			labelTotalCost.Text = $"Total Cost: ${numericUpDown1.Value}";
		}

		private void numericUpDown1_ValueChanged(object sender, EventArgs e)
		{

			int totalCost = int.Parse(selectedVehicle.DailyRate.ToString());
			labelTotalCost.Text = $"Total Cost: ${numericUpDown1.Value * totalCost}";
		}

		private void buttonRentToCustomer_Click_1(object sender, EventArgs e)
		{
			dataGridViewRental.Rows.Clear();
			string newRental = $"{selectedVehicle.VehicleRego},{comboBoxCustomer.SelectedIndex},{selectedVehicle.DailyRate}";
			rentedList.Add(newRental);
			saveRentalToFile();
			populateDataGridViewRentalReport();
		}
	}
}

