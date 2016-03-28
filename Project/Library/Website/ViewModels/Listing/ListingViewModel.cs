using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Website.ViewModels.Listing
{
	public class ListingViewModel: Root.Models.Listing
	{
		public string AccountName { get; set; }
		public string CategoryName { get; set; }
	}
}
