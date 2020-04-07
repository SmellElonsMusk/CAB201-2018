using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMRCManagement
{
	public class Customer
	{
		// Fields
		public int CustomerID { get; set; }
		//int CustomerID = -1;

		public string Title { get; set; }
		public string FirstNames { get; set; }
		public string LastName { get; set; }
		public string DateOfBirth { get; set; }

		// Enumerator Fields
		public GenderEnum Gender { get; set; }
		
		
		// This constructor should construct a customer with the provided attributes.
		public Customer (int CID,string title, string firstNames, string lastName, GenderEnum gender, string dateOfBirth)
		{
			
			CustomerID = CID;
			Title = title;
			FirstNames = firstNames;
			LastName = lastName;
			Gender = gender;
			this.DateOfBirth = dateOfBirth;

		}

		// Enumerators
		public enum GenderEnum
		{
			Male,
			Female
		};
		
		// Methods ----------------------------------------------------------------------------------------------------------

		/* This method should return a CSV representation of the customer that is consistent
	     	with the provided data files.
		*/

		//public string ToCSVString()
		//{

			//string csv = string.Join(",", myList.Select(x =>.ToString().ToArray());
		//}

		// This method should return a string representation of the attributes of the customer.
		public override string ToString()
		{
			return CustomerID+","+Title+","+FirstNames+","+LastName+","+Gender+","+DateOfBirth;
		}
		
	}
}
