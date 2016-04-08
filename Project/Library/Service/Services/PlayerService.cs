using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Root.Data.Infrastructure;
using Root.Data.Repository;
using Root.Models;
using Website.ViewModels.Player;

namespace Service.Services
{
	public interface IPlayerService
	{
		PlayerViewModel GetCardInfo(string id);
		PlayerDatatable GetCardDatatable(int page, int itemsPerPage, string sortBy, bool reverse, string searchValue);
		PlayerCardGroupDatatable GetPlayerCardGroupDatatable(int page, int itemsPerPage, string sortBy, bool reverse, string playerId, string searchValue);
		PlayerStarDatatable GetPlayerStarDatatable(int page, int itemsPerPage, string sortBy, bool reverse, string playerId, string searchValue);
		PlayerAssetDatatable GetPlayerAssetDatatable(int page, int itemsPerPage, string sortBy, bool reverse, string playerId, string searchValue);
		void UpdateCard(PlayerViewModel card);
		void DeleteCard(string id);
		void SavePlayer();
	}

	public class PlayerService: IPlayerService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IPlayerRepository _playerRepository;
		public PlayerService(IUnitOfWork unitOfWork, IPlayerRepository playerRepository)
		{
			this._playerRepository = playerRepository;
			this._unitOfWork = unitOfWork;
		}

		public PlayerViewModel GetCardInfo(string id)
		{
			var player = _playerRepository.Query(p => p.Id == id).FirstOrDefault();
			if (player != null)
			{
				var vmPlayer = Mapper.Map<Player, PlayerViewModel>(player);
				return vmPlayer;
			}
			return null;
		}

		public PlayerDatatable GetCardDatatable(int page, int itemsPerPage, string sortBy, bool reverse, string searchValue)
		{
			throw new NotImplementedException();
		}

		public PlayerCardGroupDatatable GetPlayerCardGroupDatatable(int page, int itemsPerPage, string sortBy, bool reverse,
			string playerId, string searchValue)
		{
			throw new NotImplementedException();
		}

		public PlayerStarDatatable GetPlayerStarDatatable(int page, int itemsPerPage, string sortBy, bool reverse, string playerId,
			string searchValue)
		{
			throw new NotImplementedException();
		}

		public PlayerAssetDatatable GetPlayerAssetDatatable(int page, int itemsPerPage, string sortBy, bool reverse, string playerId,
			string searchValue)
		{
			throw new NotImplementedException();
		}

		public void UpdateCard(PlayerViewModel card)
		{
			throw new NotImplementedException();
		}

		public void DeleteCard(string id)
		{
			throw new NotImplementedException();
		}

		public void SavePlayer()
		{
			_unitOfWork.Commit();
		}
	}
}
