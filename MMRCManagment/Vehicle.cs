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
	/// The following provides a partial specification of the classes required to create a prototype for the
	/// MRRC program.For all classes you may implement additional private methods and parameters.You
	/// may also implement additional public methods and parameters where appropriate and necessary to
	/// complete the functionality.For example, you may want to implement additional public methods in
	/// the Vehicle class which assist in the search functionality.
	/// For each class, getters and setters may be replaced with appropriate Properties.In general getters
	/// and setters are not specified.
	/// 
	/// Author: Waldo Fouche, n9950095
	/// Date:	May 2018
	/// 
	/// </summary>

	public class Vehicle
	{
		// Public Get Setters
		public string VehicleRego { get; set; }
		public string VehicleClassValue { get; private set; }
		public string TransmissionTypeValue { get; private set; }
		public string Make { get; set; }
		public string Model { get; set; }
		public int Year { get; set; }
		public int EngineSize { get; set; }
		public bool Turbo { get; set; }
		public bool GPS { get; set; }
		public bool SunRoof { get; set; }
		public double DailyRate { get; set; }
		public string Colour { get; set; }
		public int NumSeats { get; set; }

		// Enumumerator Fields
		public FuelTypeEnum FuelType { get; set; }
		public VehicleClassEnum VehicleClass { get;  set; }
		public TransmissionTypeEnum TransmissionType { get;  set; }

		// End of Parameter Definitions

		// ---------------------------------------------------------------------

		/// <summary>
		/// This constructor provides only the mandatory parameters of the vehicle.
		/// Others are set based on the defaults of each class.
		/// 
		///	Overall defaults: Unless otherwise specified by the class, vehicles default to four
		///	seats, automatic transmission, petrol fuel, no GPS, no sun roof, and a black colour.
		///	
		///	Economy vehicles: have automatic transmission only.Default rental cost per day is $50.
		///	
		///	Family vehicles: can have manual or automatic transmission. Default rental cost per day is $80
		/// 
		///	Luxury vehicles: has GPS and a sunroof. Default rental cost per day is $120
		///	Commercial vehicle: has diesel engine by default. Default rental cost per day is $130
		/// </summary>

		public Vehicle(string vehicleRego,  string make, string model, int year, VehicleClassEnum vehicleClass)
		{
			VehicleRego = vehicleRego;
			Make = make;
			Model = model;
			Year = year;
			NumSeats = 4;
			Colour = "Black";
			GPS = false;
			SunRoof = false;
			EngineSize = 4;
			Turbo = false;
			TransmissionType = TransmissionTypeEnum.Automatic;
			FuelType = FuelTypeEnum.Petrol;
			VehicleClass = vehicleClass;
			// Nested if.
			if (VehicleClass == VehicleClassEnum.Economy){
				DailyRate = 50.00;
			}
			else if (VehicleClass == VehicleClassEnum.Family){
				DailyRate = 80.00;
				//TransmissionType = (TransmissionTypeEnum)Enum.Parse(typeof(TransmissionTypeEnum), TransmissionTypeValue);
			}
			else if (VehicleClass == VehicleClassEnum.Luxury){
				DailyRate = 120.00;
				GPS = true;
				SunRoof = true;
			}
			else if (VehicleClass == VehicleClassEnum.Commercial){
				DailyRate = 130.00;
				FuelType = FuelTypeEnum.Diesel;
			}// End Nested if.
		}// end of previous Constructor - Vehicle.

		// ---------------------------------------------------------------------


		/// This constructor provides values for all parameters of the vehicle.
		public Vehicle(string vehicleRego,  string make, string model, int year, VehicleClassEnum Class, int numSeats, TransmissionTypeEnum Transmission, 
			int engineSize, bool turbo, FuelTypeEnum Fuel, bool gps, bool sunRoof, string colour, double dailyRate){
			VehicleRego = vehicleRego;
			Make = make;
			Model = model;
			Year = year;
			NumSeats = numSeats;
			GPS = gps;
			SunRoof = sunRoof;
			// if no colour is selected
			Colour = "Black";
			Colour = colour;
			TransmissionType = Transmission;
			EngineSize = engineSize;
			Turbo = turbo;
			
			

			/// Determine Daily Rate and set vehicle class overides
			// Nested if.
			if (Class == VehicleClassEnum.Economy){
				VehicleClass = Class;
				DailyRate = 50.00;
				FuelType = FuelTypeEnum.Petrol;
				TransmissionType = TransmissionTypeEnum.Automatic;

			}
			else if (Class == VehicleClassEnum.Family){
				VehicleClass = Class;
				DailyRate = 80.00;

			}
			else if (Class == VehicleClassEnum.Sport)
			{
				VehicleClass = Class;
				DailyRate = 130.00;

			}
			else if (Class == VehicleClassEnum.OffRoad)
			{
				VehicleClass = Class;
				DailyRate = 90.00;

			}
			else if (Class == VehicleClassEnum.Luxury){
				VehicleClass = Class;
				DailyRate = 120.00;
				GPS = true;
				SunRoof = true;

			}
			else if (Class == VehicleClassEnum.Commercial){
				VehicleClass = Class;
				DailyRate = 130.00;
				FuelType = FuelTypeEnum.Diesel;

			}// End Nested if.
		} // end of previous Constructor - Vehicle.

		// ---------------------------------------------------------------------

		/// Enumerators
		public enum VehicleClassEnum{
			Economy,
			Family,
			Luxury,
			OffRoad,
			Sport,
			Commercial
		}; // End Enumerator - VehicleClass
		public enum TransmissionTypeEnum{
			Automatic,
			Manual
		}; // End Enumerator - TransmissionType
		public enum FuelTypeEnum{
			Petrol,
			Diesel
		}; // End Enumerator - FuelType

		// --------------------------------------------------------------------- 

		/// Methods

		/// <summary>
		/// This method should return a CSV representation of the vehicle that is consistent with the provided data files.
		/// </summary>
		public string ToCSVString(){
			return ToString();
		} // end of previous method - ToCSVString()

		// ---------------------------------------------------------------------

		/// <summary>
		/// This method should return a string representation of the attributes of the vehicle.
		/// </summary>
		public override string ToString(){
			return VehicleRego + "," + Make + "," + Model + "," + Year + "," + VehicleClass + "," + 
				NumSeats + "," + TransmissionType + "," + EngineSize +","+ Turbo + ","+ FuelType + "," + GPS+ "," + SunRoof + "," 
				+ Colour + "," + DailyRate;
		}// end of previous method - ToString()

		// ---------------------------------------------------------------------

		/// <summary>
		/// This method should return a list of strings which represent each attribute. Values
		/// should be made to be unique, e.g.numSeats should not be written as ‘4’ but as ‘4-
		/// Seater’, sunroof should not be written as ‘True’ but as ‘sunroof’ or with no string
		/// added if there is no sunroof.Vehicle rego, class, make, model, year, transmission
		/// type, fuel type, daily rate, and colour can all be assumed to not overlap(i.e. if
		/// the make ‘Mazda’ exists, ‘Mazda’ will not exist in other attributes e.g.there is
		/// no model named ‘Mazda’. Similarly, if the colour ‘red’ exists, there is no ‘red’
		/// make.You do not need to maintain this restriction, only assume it is true.)
		/// </summary>
		public List<string> GetAttributeList()
		{
			List<string> attributes = new List<string>();

			attributes.Add(VehicleRego);
			attributes.Add(Make);
			attributes.Add(Model);
			attributes.Add(Year.ToString());
			attributes.Add(VehicleClass.ToString());
			attributes.Add(NumSeats + "-Seater");
			attributes.Add(TransmissionType.ToString());
			attributes.Add(EngineSize + "-Cylinder");
			attributes.Add(Turbo.ToString());
			attributes.Add(FuelType.ToString());
			attributes.Add(GPS.ToString());
			attributes.Add(SunRoof.ToString());
			attributes.Add(Colour);
			attributes.Add(DailyRate.ToString());

			return attributes;

		}// end of previous method - GetAttributeList()

		// ---------------------------------------------------------------------

	} // End of Classs - Vehicles
} 
