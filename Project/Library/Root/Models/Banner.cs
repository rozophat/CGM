using System;

namespace Root.Models
{
	public class Banner
	{
		public int Id { get; set; }
		public int AccountId { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public string DestinationType { get; set; }
		public string Destination { get; set; }
		public int CategoryId { get; set; }
		public bool Paid { get; set; }
		public string TransactionId { get; set; }
		public string City { get; set; }
		public string ProvinceState { get; set; }
		public string Country { get; set; }
		public string Active { get; set; }
		public DateTime StartDateTime { get; set; }
		public DateTime EndDateTime { get; set; }
		public string GPSLatitude { get; set; }
		public string GPSLongitude { get; set; }
	}
}
