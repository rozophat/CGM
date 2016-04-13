using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Root.Data.Infrastructure;
using Root.Data.Repository;
using Root.Models;
using Website.ViewModels.Asset;
using Website.ViewModels.CardGroup;

namespace Service.Services
{
    public interface IAssetService
    {
        IEnumerable<AssetViewModel> GetAutoSuggestAsset(string value);
        void SaveCardGroup();
    }

    public class AssetService: IAssetService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAssetRepository _assetRepository;

        public AssetService(IUnitOfWork unitOfWork, IAssetRepository assetRepository)
        {
            this._assetRepository = assetRepository;
            this._unitOfWork = unitOfWork;
        }

        public IEnumerable<AssetViewModel> GetAutoSuggestAsset(string value)
        {
            var assets = _assetRepository.Query(p => p.Name.Contains(value));
            if (assets != null)
            {
                var destination = Mapper.Map<IEnumerable<Asset>, IEnumerable<AssetViewModel>>(assets);
                return destination;
            }
            return null;
        }

        public void SaveCardGroup()
        {
            _unitOfWork.Commit();
        }
    }
}
