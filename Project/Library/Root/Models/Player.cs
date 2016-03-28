using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Root.Models
{
	public class Player
	{
		public string Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string DeviceToken { get; set; }
		public string FacebookId { get; set; }
		public string FullName { get; set; }
		public string NickName { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		public string City { get; set; }
		public string ProvinceState { get; set; }
		public string Country { get; set; }
		public DateTime DOB { get; set; }
		public int Points { get; set; }
		public int PointsToWinStar { get; set; }
	}
}
