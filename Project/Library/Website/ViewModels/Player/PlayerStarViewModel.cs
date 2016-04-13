using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Root.Models;

namespace Website.ViewModels.Player
{
	public class PlayerStarViewModel: PlayerStar
	{
		public string PCGPlayerFullName { get; set; }
		public string PCGGroupName { get; set; }
	}
}
