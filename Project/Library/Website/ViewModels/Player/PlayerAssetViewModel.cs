﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Root.Models;

namespace Website.ViewModels.Player
{
	public class PlayerAssetViewModel: PlayerAsset
	{
		public string PlayerFullName { get; set; }
		public string AssetName { get; set; }
	}
}
