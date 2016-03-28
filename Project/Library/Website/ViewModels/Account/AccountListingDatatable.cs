using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Website.ViewModels.Listing;

namespace Website.ViewModels.Account
{
	public class AccountListingDatatable
	{
		public List<ListingViewModel> Data { get; set; }
		public int Total { get; set; }
	}
}
