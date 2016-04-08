using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
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
			var vmPlayer = _playerService.GetCardInfo(id);
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
			var cardTable = _playerService.GetCardDatatable(page, itemsPerPage, sortBy, reverse, search);
			if (cardTable == null)
			{
				return NotFound();
			}
			return Ok(cardTable);
		}

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

		public void Put(PlayerViewModel vmcard)
		{
			_playerService.UpdateCard(vmcard);
		}

		public void Delete(string id)
		{
			_playerService.DeleteCard(id);
		}
	}
}
