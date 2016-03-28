namespace Root.Models
{
	public class Listing
	{
		public int Id { get; set; }
		public int AccountId { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public string ListingType { get; set; }
		public bool Paid { get; set; }
		public string TransactionId { get; set; }
		public string Active { get; set; }
		public int CategoryId { get; set; }
		public string GPSLatitude { get; set; }
		public string GPSLongitude { get; set; }
	}
}
