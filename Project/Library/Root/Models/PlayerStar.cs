using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Root.Models
{
	public class PlayerStar
	{
		public string Id { get; set; }
		public string PlayerId { get; set; }
		public DateTime CreateDate { get; set; }
		public bool Used { get; set; }
		public string PlayerCardGroupId { get; set; }
		public bool IsPurchased { get; set; }
		public string PurchaseTransactionId { get; set; }
	}
}
