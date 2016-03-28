using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Root.Models
{
	public class Account
	{
		public int Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string PhoneNumber { get; set; }
		public string Email { get; set; }

		public DateTime? DOB { get; set; }
		public int YearGroupAge { get; set; }
		public string Sex { get; set; }
		public string AddressLine1 { get; set; }
		public string AddressLine2 { get; set; }
		public string City { get; set; }
		public string ProvinceState { get; set; }
		public string Country { get; set; }
		public string PostalZip { get; set; }
		public string GPSLatitude { get; set; }
		public string GPSLongitude { get; set; }

		public string FBId { get; set; }
		public string GId { get; set; }
		public string Password { get; set; }
		public string AboutMe { get; set; }
	}
}
