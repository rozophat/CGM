using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Root.Data.Infrastructure;
using Root.Data.Repository;
using Root.Models;
using Website.ViewModels.Card;

namespace Service.Services
{
	public interface ICardService
	{
        CardViewModel GetCardInfo(string id);
        CardDatatable GetCardDatatable(int page, int itemsPerPage, string sortBy, bool reverse, string searchValue);
        void CreateCard(CardViewModel card);
        void UpdateCard(CardViewModel card);
        void DeleteCard(string id);
        void SaveCard();
	}

	public class CardService : ICardService
	{
		private readonly IUnitOfWork _unitOfWork;
        private readonly ICardRepository _cardRepository;

        public CardService(IUnitOfWork unitOfWork, ICardRepository cardRepository)
        {
            this._cardRepository = cardRepository;
            this._unitOfWork = unitOfWork;
        }

        public CardViewModel GetCardInfo(string id)
        {
            var card = _cardRepository.Query(p => p.Id == id).FirstOrDefault();
            if (card != null)
            {
                var vmCard = Mapper.Map<Card, CardViewModel>(card);
                return vmCard;
            }
            return null;
        }

        public CardDatatable GetCardDatatable(int page, int itemsPerPage, string sortBy, bool reverse, string searchValue)
        {
            var cards = _cardRepository.GetAllQueryable();
            // searching
            if (!string.IsNullOrWhiteSpace(searchValue))
            {
                searchValue = searchValue.ToLower();
                cards = cards.Where(p => p.Type == searchValue);
            }

            // sorting (done with the System.Linq.Dynamic library available on NuGet)
            var cardsOrdered = cards.OrderBy(sortBy + (reverse ? " descending" : ""));

            // paging
            var cardsPaged = cardsOrdered.Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();

            var destination = Mapper.Map<List<Card>, List<CardViewModel>>(cardsPaged);
            var cardDatatable = new CardDatatable()
            {
                Data = destination,
                Total = cards.Count()
            };
            return cardDatatable;
        }

        public void CreateCard(CardViewModel vmCard)
        {
            var cards = Mapper.Map<CardViewModel, Card>(vmCard);
            cards.Id = Guid.NewGuid().ToString();
            _cardRepository.Add(cards);
            SaveCard();
        }

        public void UpdateCard(CardViewModel vmCard)
        {
            var card = Mapper.Map<CardViewModel, Card>(vmCard);
            _cardRepository.Update(card);
            SaveCard();
        }

        public void DeleteCard(string id)
        {
            _cardRepository.Delete(p => p.Id == id);
            SaveCard();
        }
		public void SaveCard()
		{
			_unitOfWork.Commit();
		}
	}
}
