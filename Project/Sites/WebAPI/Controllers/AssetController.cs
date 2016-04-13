using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Service.Services;
using Website.ViewModels.Asset;

namespace WebAPI.Controllers
{
    [Authorize]
    public class AssetController : ApiController
    {
        public IAssetService _assetService;
        public AssetController() { }

        public AssetController(IAssetService assetService)
        {
            this._assetService = assetService;
        }

        [Route("api/Asset/GetAutoSuggestAsset")]
        public IEnumerable<AssetViewModel> GetAutoSuggestAsset(string value)
        {
            return _assetService.GetAutoSuggestAsset(value);
        }
    }
}
