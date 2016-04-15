using Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Website.ViewModels.CardGroup;

namespace WebAPI.Controllers
{
    [Authorize]
    public class CardGroupController : ApiController
    {
        public ICardGroupService _cardGroupService;

        public CardGroupController() { }

        public CardGroupController(ICardGroupService cardGroupService)
        {
            this._cardGroupService = cardGroupService;
        }

        public IHttpActionResult Get(string id)
        {
            var vmCardGroup = _cardGroupService.GetCardGroupInfo(id);
            if (vmCardGroup == null)
            {
                return NotFound();
            }
            return Ok(vmCardGroup);
        }

        [Route("api/CardGroup/GetAutoSuggestCardGroup")]
        public IEnumerable<CardGroupViewModel> GetAutoSuggestCardGroup(string value)
        {
            return _cardGroupService.GetAutoSuggestCardGroup(value);
        }

        [Route("api/CardGroup/Datatable")]
        public IHttpActionResult Get(
                  int page = 1,
                  int itemsPerPage = 10,
                  string sortBy = "Name",
                  bool reverse = false,
                  string search = null)
        {
            var cardGroupTable = _cardGroupService.GetCardGroupDatatable(page, itemsPerPage, sortBy, reverse, search);
            if (cardGroupTable == null)
            {
                return NotFound();
            }
            return Ok(cardGroupTable);
        }

        [HttpGet]
        [Route("api/CardGroup/CardDatatable")]
        public IHttpActionResult CardDatatable(
                  int page = 1,
                  int itemsPerPage = 10,
                  string sortBy = "CreatedDate",
                  bool reverse = false,
                  string id = "",
                  string type = "",
                  string difficulty = "",
                  string search = null)
        {
            var cardTable = _cardGroupService.GetCardDatatable(page, itemsPerPage, sortBy, reverse, id, type, difficulty, search);
            if (cardTable == null)
            {
                return NotFound();
            }
            return Ok(cardTable);
        }

		[HttpGet]
		[Route("api/CardGroup/AllCardDatatable")]
		public IHttpActionResult AllCardDatatable()
		{
			var cardTable = _cardGroupService.GetAllCardDatatable();
			if (cardTable == null)
			{
				return NotFound();
			}
			return Ok(cardTable);
		}

		[HttpGet]
		[Route("api/CardGroup/UpdateCardsToGroup")]
		public IHttpActionResult UpdateCardsToGroup(string selectedIds, string groupId)
		{
			var status = _cardGroupService.UpdateCardsToGroup(selectedIds, groupId);
			
			return Ok(status);
		}

		
        public void Post(CardGroupViewModel cardGroup)
        {
            _cardGroupService.CreateCardGroup(cardGroup);
        }

        public void Put(CardGroupViewModel vmcardGroup)
        {
            _cardGroupService.UpdateCardGroup(vmcardGroup);
        }

        public void Delete(string id)
        {
            _cardGroupService.DeleteCardGroup(id);
        }

        [HttpDelete]
        [Route("api/CardGroup/DeleteCardFromGroup/{id}")]
        public void DeleteCardFromGroup(string id)
        {
            _cardGroupService.DeleteCardFromGroup(id);
        }
    }
}
