using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Root.Models.Mapping
{
	public class CardGroup
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public string Decription { get; set; }
		public DateTime CreateDate { get; set; }
		public DateTime UpdatedDate { get; set; }
		public string AppleProductCode { get; set; }
		public string GoogleProductCode { get; set; }
		public string Type { get; set; }
		public int PriceInStar { get; set; }
		public decimal Price { get; set; }
		public bool Active { get; set; }
	}
}
