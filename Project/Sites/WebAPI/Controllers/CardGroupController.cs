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
    }
}
