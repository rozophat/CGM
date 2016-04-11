using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Root.Models;
using Service.Services;
using Website.ViewModels.Player;

namespace WebAPI.Controllers
{
	[Authorize]
    public class PlayerController : ApiController
	{
		public IPlayerService _playerService;
		public PlayerController() { }

		public PlayerController(IPlayerService playerService)
        {
			this._playerService = playerService;
        }

		public IHttpActionResult Get(string id)
		{
			var vmPlayer = _playerService.GetPlayerInfo(id);
			if (vmPlayer == null)
			{
				return NotFound();
			}
			return Ok(vmPlayer);
		}

		[HttpGet]
		[Route("api/Player/GetPlayerCardGroupInfo")]
		public IHttpActionResult GetPlayerCardGroup(string id)
		{
			var vmPlayerCardGroup = _playerService.GetPlayerCardGroupInfo(id);
			if (vmPlayerCardGroup == null)
			{
				return NotFound();
			}
			return Ok(vmPlayerCardGroup);
		}

		[HttpGet]
		[Route("api/Player/GetPlayerStarInfo")]
		public IHttpActionResult GetPlayerStarInfo(string id)
		{
			var vmPlayer = _playerService.GetPlayerInfo(id);
			if (vmPlayer == null)
			{
				return NotFound();
			}
			return Ok(vmPlayer);
		}

		[HttpGet]
		[Route("api/Player/GetPlayerAssetInfo")]
		public IHttpActionResult GetPlayerAssetInfo(string id)
		{
			var vmPlayer = _playerService.GetPlayerInfo(id);
			if (vmPlayer == null)
			{
				return NotFound();
			}
			return Ok(vmPlayer);
		}

		[Route("api/Player/Datatable")]
		public IHttpActionResult Get(
				  int page = 1,
				  int itemsPerPage = 10,
				  string sortBy = "FullName",
				  bool reverse = false,
				  string search = null)
		{
			var playerTable = _playerService.GetPlayerDatatable(page, itemsPerPage, sortBy, reverse, search);
			if (playerTable == null)
			{
				return NotFound();
			}
			return Ok(playerTable);
		}

		[HttpGet]
		[Route("api/Player/PlayerCardGroupDatatable")]
		public IHttpActionResult PlayerCardGroupDatatable(
				  int page = 1,
				  int itemsPerPage = 10,
				  string sortBy = "FullName",
				  bool reverse = false,
				  string playerId = "",
				  string search = null)
		{
			var cardGroupTable = _playerService.GetPlayerCardGroupDatatable(page, itemsPerPage, sortBy, reverse, playerId, search);
			if (cardGroupTable == null)
			{
				return NotFound();
			}
			return Ok(cardGroupTable);
		}

		[HttpGet]
		[Route("api/Player/PlayerStarDatatable")]
		public IHttpActionResult PlayerStarDatatable(
				  int page = 1,
				  int itemsPerPage = 10,
				  string sortBy = "CreateDate",
				  bool reverse = false,
				  string playerId = "",
				  string search = null)
		{
			var starTable = _playerService.GetPlayerStarDatatable(page, itemsPerPage, sortBy, reverse, playerId, search);
			if (starTable == null)
			{
				return NotFound();
			}
			return Ok(starTable);
		}

		[HttpGet]
		[Route("api/Player/PlayerAssetDatatable")]
		public IHttpActionResult PlayerAssetDatatable(
				  int page = 1,
				  int itemsPerPage = 10,
				  string sortBy = "CreateDate",
				  bool reverse = false,
				  string playerId = "",
				  string search = null)
		{
			var assetTable = _playerService.GetPlayerAssetDatatable(page, itemsPerPage, sortBy, reverse, playerId, search);
			if (assetTable == null)
			{
				return NotFound();
			}
			return Ok(assetTable);
		}

		public void Put(PlayerViewModel vmPlayer)
		{
			_playerService.UpdatePlayer(vmPlayer);
		}

		[HttpPut]
		[Route("api/Player/UpdatePlayerCardGroup")]
		public void UpdatePlayerCardGroup(PlayerCardGroupViewModel vmPlayerCardGroup)
		{
			_playerService.UpdatePlayerCardGroup(vmPlayerCardGroup);
		}

		[HttpPut]
		[Route("api/Player/UpdatePlayerStar")]
		public void UpdatePlayerStar(PlayerStarViewModel vmPlayerStar)
		{
			_playerService.UpdatePlayerStar(vmPlayerStar);
		}

		[HttpPut]
		[Route("api/Player/UpdatePlayerAsset")]
		public void UpdatePlayerAsset(PlayerAssetViewModel vmPlayerAsset)
		{
			_playerService.UpdatePlayerAsset(vmPlayerAsset);
		}

		public void Delete(string id)
		{
			_playerService.DeletePlayer(id);
		}

		[HttpDelete]
		[Route("api/Profile/DeletePlayerCardGroup/{id}")]
		public void DeletePlayerCardGroup(string id)
		{
			_playerService.DeletePlayerCardGroup(id);
		}

		[HttpDelete]
		[Route("api/Profile/DeletePlayerStar/{id}")]
		public void DeletePlayerStar(string id)
		{
			_playerService.DeletePlayerStar(id);
		}

		[HttpDelete]
		[Route("api/Profile/DeleteAssetStar/{id}")]
		public void DeleteAssetStar(string id)
		{
			_playerService.DeletePlayerAsset(id);
		}
	}
}
