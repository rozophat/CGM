using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Website.ViewModels.Account
{
	public class AccountKeywordViewModel: Root.Models.AccountKeyword
	{
		public string KeywordName { get; set; }
		public int? KeywordType { get; set; }
	}
}
