using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Root.Models
{
	public class PlayerCardGroup
	{
		public string Id { get; set; }
		public string PlayerId { get; set; }
		public string CardGroupId { get; set; }
		public DateTime PurchasedDate { get; set; }
		public string PurchaseSource { get; set; }
		public string TransactionId { get; set; }
		public string StoreCost { get; set; }
		public int StarsCost { get; set; }
	}
}
