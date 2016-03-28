using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Root.Models
{
	public class Category
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int? ParentId { get; set; }
		public int Type { get; set; }
		public string Name_FR { get; set; }
		public string Name_ES { get; set; }
		public string Name_PT { get; set; }
	}
}
