namespace Root.Models
{
	public class BannerImage
	{
		public int Id { get; set; }
		public int BannerId { get; set; }
		public string FileName { get; set; }
		public int ImageType { get; set; }
	}
}
