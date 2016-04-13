using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Root.Data.Infrastructure;
using Root.Data.Repository;
using Root.Models;
using Website.ViewModels.Card;
using Website.ViewModels.Player;

namespace Service.Services
{
	public interface IPlayerService
	{
		PlayerViewModel GetPlayerInfo(string id);
		PlayerCardGroupViewModel GetPlayerCardGroupInfo(string id);
		IEnumerable<PlayerCardGroupViewModel> GetAutoSuggestPlayerCardGroup(string value);
		PlayerStarViewModel GetPlayerStarInfo(string id);
		PlayerAssetViewModel GetAssetInfo(string id);
		PlayerDatatable GetPlayerDatatable(int page, int itemsPerPage, string sortBy, bool reverse, string searchValue);
		PlayerCardGroupDatatable GetPlayerCardGroupDatatable(int page, int itemsPerPage, string sortBy, bool reverse, string playerId, string searchValue);
		PlayerStarDatatable GetPlayerStarDatatable(int page, int itemsPerPage, string sortBy, bool reverse, string playerId, string searchValue);
		PlayerAssetDatatable GetPlayerAssetDatatable(int page, int itemsPerPage, string sortBy, bool reverse, string playerId, string searchValue);
		void UpdatePlayer(PlayerViewModel vmPlyaer);
		void UpdatePlayerCardGroup(PlayerCardGroupViewModel vmPlayerCardGroup);
		void UpdatePlayerStar(PlayerStarViewModel vmPlayerStar);
		void UpdatePlayerAsset(PlayerAssetViewModel vmPlayerAsset);
		void DeletePlayer(string id);
		void DeletePlayerCardGroup(string id);
		void DeletePlayerStar(string id);
		void DeletePlayerAsset(string id);
		void SavePlayer();
	}

	public class PlayerService : IPlayerService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IPlayerRepository _playerRepository;
		private readonly IPlayerCardGroupRepository _playerCardGroupRepository;
		private readonly IPlayerAssetRepository _playerAssetRepository;
		private readonly IPlayerStarRepository _playerStarRepository;
		private readonly IAssetRepository _assetRepository;
		private readonly ICardGroupRepository _cardGroupRepository;
		private readonly ICardRepository _cardRepository;

		public PlayerService(IUnitOfWork unitOfWork, IPlayerRepository playerRepository,
							IPlayerCardGroupRepository playerCardGroupRepository, ICardGroupRepository cardGroupRepository,
							IPlayerAssetRepository playerAssetRepository, IPlayerStarRepository playerStarRepository,
							IAssetRepository assetRepository, ICardRepository cardRepository)
		{
			this._playerRepository = playerRepository;
			this._playerCardGroupRepository = playerCardGroupRepository;
			this._cardGroupRepository = cardGroupRepository;
			this._playerAssetRepository = playerAssetRepository;
			this._playerStarRepository = playerStarRepository;
			this._assetRepository = assetRepository;
			this._cardRepository = cardRepository;
			this._unitOfWork = unitOfWork;
		}

		public PlayerViewModel GetPlayerInfo(string id)
		{
			var player = _playerRepository.Query(p => p.Id == id).FirstOrDefault();
			if (player != null)
			{
				var vmPlayer = Mapper.Map<Player, PlayerViewModel>(player);
				return vmPlayer;
			}
			return null;
		}

		public PlayerCardGroupViewModel GetPlayerCardGroupInfo(string id)
		{
			var cgplayer = (from p in _playerCardGroupRepository.GetAllQueryable()
						   join g in _cardGroupRepository.GetAllQueryable() on p.CardGroupId equals g.Id into pg
						   from g in pg.DefaultIfEmpty()
						   where p.Id == id
						   select new PlayerCardGroupViewModel()
						   {
							   Id = p.Id,
							   PlayerId = p.PlayerId,
							   CardGroupId = p.CardGroupId,
							   GroupName = g.Name,
							   PurchasedDate = p.PurchasedDate,
							   PurchaseSource = p.PurchaseSource,
							   TransactionId = p.TransactionId,
							   StoreCost = p.StoreCost,
							   StarsCost = p.StarsCost
						   }).FirstOrDefault();
			return cgplayer;
		}

		public IEnumerable<PlayerCardGroupViewModel> GetAutoSuggestPlayerCardGroup(string value)
		{
			var cgPlayers = from p in _playerCardGroupRepository.GetAllQueryable()
							join l in _playerRepository.GetAllQueryable() on p.PlayerId equals l.Id into pl
							from l in pl.DefaultIfEmpty()
							join g in _cardGroupRepository.GetAllQueryable() on p.CardGroupId equals g.Id into plg
							from g in plg.DefaultIfEmpty()
							where
								l.FullName.Contains(value) || l.FirstName.Contains(value) || l.LastName.Contains(value) || l.Email.Contains(value) ||
								g.Name.Contains(value)
							select new PlayerCardGroupViewModel()
							{
								Id = p.Id,
								PlayerId = p.PlayerId,
								PlayerFullName = l.FullName,
								CardGroupId = p.CardGroupId,
								GroupName = g.Name
							};

			return cgPlayers;
		}

		public PlayerStarViewModel GetPlayerStarInfo(string id)
		{
			var starPlayer = (from p in _playerStarRepository.GetAllQueryable()
							  join g in _playerCardGroupRepository.GetAllQueryable() on p.PlayerCardGroupId equals g.Id into pg
							  from g in pg.DefaultIfEmpty()
							  join l in _playerRepository.GetAllQueryable() on g.PlayerId equals l.Id into pgl
							  from l in pgl.DefaultIfEmpty()
							  join c in _cardGroupRepository.GetAllQueryable() on g.CardGroupId equals c.Id into pglc
							  from c in pglc.DefaultIfEmpty()
							  where p.Id == id
							  select new PlayerStarViewModel()
							  {
								  Id = p.Id,
								  PlayerId = p.PlayerId,
								  CreatedDate = p.CreatedDate,
								  Used = p.Used,
								  PlayerCardGroupId = p.PlayerCardGroupId,
								  PCGPlayerFullName = l.FullName,
								  PCGGroupName = c.Name,
								  IsPurchased = p.IsPurchased,
								  PurchaseTransactionId = p.PurchaseTransactionId
							  }).FirstOrDefault();

			return starPlayer;
		}

		public PlayerAssetViewModel GetAssetInfo(string id)
		{

			var assetPlayer = (from p in _playerAssetRepository.GetAllQueryable()
							   join a in _assetRepository.GetAllQueryable() on p.AssetId equals a.Id into pa
							   from a in pa.DefaultIfEmpty()
							   join q in _cardRepository.GetAllQueryable() on p.UsedCardId equals q.Id into paq
							   from q in paq.DefaultIfEmpty()
							   where p.Id == id
							   select new PlayerAssetViewModel()
							   {
								   Id = p.Id,
								   AssetId = p.AssetId,
								   AssetName = a.Name,
								   PlayerId = p.PlayerId,
								   CreatedDate = p.CreatedDate,
								   Used = p.Used,
								   UsedDate = p.UsedDate,
								   UsedCardId = p.UsedCardId,
								   Question1 = q.Question1,
								   Question2 = q.Question2,
								   Question3 = q.Question3
							   }).FirstOrDefault();

			return assetPlayer;
		}

		public PlayerDatatable GetPlayerDatatable(int page, int itemsPerPage, string sortBy, bool reverse, string searchValue)
		{
			var players = _playerRepository.GetAllQueryable();
			// searching
			if (!string.IsNullOrWhiteSpace(searchValue))
			{
				searchValue = searchValue.ToLower();
				players = players.Where(p => p.FullName.Contains(searchValue) || p.NickName.Contains(searchValue) || p.Email == searchValue);
			}

			// sorting (done with the System.Linq.Dynamic library available on NuGet)
			var playersOrdered = players.OrderBy(sortBy + (reverse ? " descending" : ""));

			// paging
			var playersPaged = playersOrdered.Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();

			var destination = Mapper.Map<List<Player>, List<PlayerViewModel>>(playersPaged);
			var cardDatatable = new PlayerDatatable()
			{
				Data = destination,
				Total = players.Count()
			};
			return cardDatatable;
		}

		public PlayerCardGroupDatatable GetPlayerCardGroupDatatable(int page, int itemsPerPage, string sortBy, bool reverse,
			string playerId, string searchValue)
		{
			var cgPlayers = from p in _playerCardGroupRepository.GetAllQueryable()
							join q in _playerRepository.GetAllQueryable() on p.PlayerId equals q.Id into pq
							from q in pq.DefaultIfEmpty()
							join c in _cardGroupRepository.GetAllQueryable() on p.CardGroupId equals c.Id into cpq
							from c in cpq.DefaultIfEmpty()
							where p.PlayerId == playerId
							select new PlayerCardGroupViewModel()
							{
								Id = p.Id,
								PlayerId = p.PlayerId,
								PlayerFullName = q.FullName,
								GroupName = c.Name,
								PurchaseSource = p.PurchaseSource,
								PurchasedDate = p.PurchasedDate,
								TransactionId = p.TransactionId,
								StoreCost = p.StoreCost,
								StarsCost = p.StarsCost
							};

			// searching
			if (!string.IsNullOrWhiteSpace(searchValue))
			{
				searchValue = searchValue.ToLower();
				cgPlayers = cgPlayers.Where(p => p.PlayerFullName.Contains(searchValue) || p.GroupName.Contains(searchValue));
			}

			// sorting (done with the System.Linq.Dynamic library available on NuGet)
			var cgPlayersOrdered = cgPlayers.OrderBy(sortBy + (reverse ? " descending" : ""));

			// paging
			var cgPlayersPaged = cgPlayersOrdered.Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();

			var cgPlayersDatatable = new PlayerCardGroupDatatable()
			{
				Data = cgPlayersPaged,
				Total = cgPlayers.Count()
			};
			return cgPlayersDatatable;
		}

		public PlayerStarDatatable GetPlayerStarDatatable(int page, int itemsPerPage, string sortBy, bool reverse, string playerId,
			string searchValue)
		{
			var starPlayers = from p in _playerStarRepository.GetAllQueryable()
							  join c in _playerCardGroupRepository.GetAllQueryable() on p.PlayerCardGroupId equals c.Id into cp
							  from c in cp.DefaultIfEmpty()
							  join q in _playerRepository.GetAllQueryable() on c.PlayerId equals q.Id into pq
							  from q in pq.DefaultIfEmpty()
							  join g in _cardGroupRepository.GetAllQueryable() on c.CardGroupId equals g.Id into cg
							  from g in cg.DefaultIfEmpty()
							  where p.PlayerId == playerId
							  select new PlayerStarViewModel()
							  {
								  Id = p.Id,
								  PlayerId = p.PlayerId,
								  PCGPlayerFullName = q.FullName,
								  PCGGroupName = g.Name,
								  PlayerCardGroupId = p.PlayerCardGroupId,
								  CreatedDate = p.CreatedDate,
								  Used = p.Used,
								  IsPurchased = p.IsPurchased,
								  PurchaseTransactionId = p.PurchaseTransactionId
							  };

			// searching
			if (!string.IsNullOrWhiteSpace(searchValue))
			{
				searchValue = searchValue.ToLower();
				starPlayers = starPlayers.Where(p => p.PCGPlayerFullName.Contains(searchValue) || p.PCGGroupName.Contains(searchValue));
			}

			// sorting (done with the System.Linq.Dynamic library available on NuGet)
			var starPlayersOrdered = starPlayers.OrderBy(sortBy + (reverse ? " descending" : ""));

			// paging
			var starPlayersPaged = starPlayersOrdered.Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();

			var cgPlayersDatatable = new PlayerStarDatatable()
			{
				Data = starPlayersPaged,
				Total = starPlayers.Count()
			};
			return cgPlayersDatatable;
		}

		public PlayerAssetDatatable GetPlayerAssetDatatable(int page, int itemsPerPage, string sortBy, bool reverse, string playerId,
			string searchValue)
		{
			var assetPlayers = from p in _playerAssetRepository.GetAllQueryable()
							   join q in _playerRepository.GetAllQueryable() on p.PlayerId equals q.Id into pq
							   from q in pq.DefaultIfEmpty()
							   join c in _cardGroupRepository.GetAllQueryable() on p.UsedCardId equals c.Id into cpq
							   from c in cpq.DefaultIfEmpty()
							   join a in _assetRepository.GetAllQueryable() on p.AssetId equals a.Id into acpq
							   from a in acpq.DefaultIfEmpty()
							   where p.PlayerId == playerId
							   select new PlayerAssetViewModel()
							   {
								   Id = p.Id,
								   PlayerId = p.PlayerId,
								   AssetId = p.AssetId,
								   AssetName = a.Name,
								   UsedCardId = p.UsedCardId,
								   CreatedDate = p.CreatedDate,
								   Used = p.Used,
								   UsedDate = p.UsedDate
							   };

			// searching
			if (!string.IsNullOrWhiteSpace(searchValue))
			{
				searchValue = searchValue.ToLower();
				assetPlayers = assetPlayers.Where(p => p.AssetName.Contains(searchValue));
			}

			// sorting (done with the System.Linq.Dynamic library available on NuGet)
			var assetPlayersOrdered = assetPlayers.OrderBy(sortBy + (reverse ? " descending" : ""));

			// paging
			var assetPlayersPaged = assetPlayersOrdered.Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();

			var assetPlayersDatatable = new PlayerAssetDatatable()
			{
				Data = assetPlayersPaged,
				Total = assetPlayers.Count()
			};
			return assetPlayersDatatable;
		}

		public void UpdatePlayer(PlayerViewModel vmPlyaer)
		{
			var player = Mapper.Map<PlayerViewModel, Player>(vmPlyaer);
			_playerRepository.Update(player);
			SavePlayer();
		}

		public void UpdatePlayerCardGroup(PlayerCardGroupViewModel vmPlayerCardGroup)
		{
			var cgPlayer = Mapper.Map<PlayerCardGroupViewModel, PlayerCardGroup>(vmPlayerCardGroup);
			_playerCardGroupRepository.Update(cgPlayer);
			SavePlayer();
		}

		public void UpdatePlayerStar(PlayerStarViewModel vmPlayerStar)
		{
			var starPlayer = Mapper.Map<PlayerStarViewModel, PlayerStar>(vmPlayerStar);
			_playerStarRepository.Update(starPlayer);
			SavePlayer();
		}

		public void UpdatePlayerAsset(PlayerAssetViewModel vmPlayerAsset)
		{
			var assetPlayer = Mapper.Map<PlayerAssetViewModel, PlayerAsset>(vmPlayerAsset);
			_playerAssetRepository.Update(assetPlayer);
			SavePlayer();
		}

		public void DeletePlayer(string id)
		{
			_playerRepository.Delete(p => p.Id == id);
			_playerCardGroupRepository.Delete(p => p.PlayerId == id);
			_playerStarRepository.Delete(p => p.PlayerId == id);
			_playerAssetRepository.Delete(p => p.PlayerId == id);
			SavePlayer();
		}

		public void DeletePlayerCardGroup(string id)
		{
			_playerCardGroupRepository.Delete(p => p.PlayerId == id);
			SavePlayer();
		}

		public void DeletePlayerStar(string id)
		{
			_playerStarRepository.Delete(p => p.PlayerId == id);
			SavePlayer();
		}

		public void DeletePlayerAsset(string id)
		{
			_playerAssetRepository.Delete(p => p.PlayerId == id);
			SavePlayer();
		}

		public void SavePlayer()
		{
			_unitOfWork.Commit();
		}
	}
}
