using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Root.Models
{
	public class PlayerAsset
	{
		public string Id { get; set; }
		public string AssetId { get; set; }
		public string PlayerId { get; set; }
		public DateTime CreatedDate { get; set; }
		public string Used { get; set; }
		public DateTime UsedDate { get; set; }
		public string UsedCardId { get; set; }
	}
}
