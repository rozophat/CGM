using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Root.Models;

namespace Website.ViewModels.Account
{
	public class AccountCommentViewModel: Root.Models.Comment
	{
		public string ByAccountName { get; set; }
	}
}
