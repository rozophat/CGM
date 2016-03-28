using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Root.Models
{
	public class PlayerLog
	{
		public string Id { get; set; }
		public string PlayerId { get; set; }
		public DateTime CreateDate { get; set; }
		public int TransactionType { get; set; }
	}
}
