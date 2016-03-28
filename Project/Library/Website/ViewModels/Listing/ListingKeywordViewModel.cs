using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Root.Models;

namespace Website.ViewModels.Listing
{
	public class ListingKeywordViewModel: ListingKeyword
	{
		public string KeywordName { get; set; }
		public int? KeywordType { get; set; }
	}
}
