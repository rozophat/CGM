using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Website.ViewModels.Category
{
	public class CategoryViewModel: Root.Models.Category
	{
		public string ParentCategoryName { get; set; }
	}
}
