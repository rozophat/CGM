namespace Root.Models
{
	public class ListingImage
	{
		public int Id { get; set; }
		public int ListingId { get; set; }
		public string FileName { get; set; }
		public int ImageType { get; set; }
	}
}
