using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Website.ViewModels.Listing
{
	public class ListingCommentViewModel : Root.Models.Comment
	{
		public string ByAccountName { get; set; }
	}
}
