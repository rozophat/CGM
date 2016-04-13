using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Service.Services;
using Website.ViewModels.Card;

namespace WebAPI.Controllers
{
    [Authorize]
    public class CardController : ApiController
    {
        public ICardService _cardService;

        public CardController() { }

        public CardController(ICardService cardService)
        {
            this._cardService = cardService;
        }

        public IHttpActionResult Get(string id)
        {
            var vmCard = _cardService.GetCardInfo(id);
            if (vmCard == null)
            {
                return NotFound();
            }
            return Ok(vmCard);
        }

		[HttpGet]
		[Route("api/Card/GetAutoSuggestCard")]
		public IEnumerable<CardViewModel> GetAutoSuggestCard(string value)
		{
			return _cardService.GetAutoSuggestCard(value);
		}

        [Route("api/Card/Datatable")]
        public IHttpActionResult Get(
                  int page = 1,
                  int itemsPerPage = 10,
                  string sortBy = "CreatedDate",
                  bool reverse = false,
                  string search = null)
        {
            var cardTable = _cardService.GetCardDatatable(page, itemsPerPage, sortBy, reverse, search);
            if (cardTable == null)
            {
                return NotFound();
            }
            return Ok(cardTable);
        }

        public void Post(CardViewModel card)
        {
            _cardService.CreateCard(card);
        }

        public void Put(CardViewModel vmcard)
        {
            _cardService.UpdateCard(vmcard);
        }

        public void Delete(string id)
        {
            _cardService.DeleteCard(id);
        }
    }
}
