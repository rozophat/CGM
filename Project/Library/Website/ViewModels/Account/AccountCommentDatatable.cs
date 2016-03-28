using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Website.ViewModels.Comment;

namespace Website.ViewModels.Account
{
	public class AccountCommentDatatable
	{
		public List<AccountCommentViewModel> Data { get; set; }
		public int Total { get; set; }
	}
}
