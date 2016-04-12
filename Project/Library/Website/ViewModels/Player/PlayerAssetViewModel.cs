using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Root.Models;

namespace Website.ViewModels.Player
{
	public class PlayerAssetViewModel: PlayerAsset
	{
		public string AssetName { get; set; }
		public string Question1 { get; set; }
		public string Question2 { get; set; }
		public string Question3 { get; set; }
		public string Type { get; set; }
		public string Difficulty { get; set; }
	}
}
