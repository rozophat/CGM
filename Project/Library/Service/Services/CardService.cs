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
		IEnumerable<CardViewModel> GetAutoSuggestCard(string value);
		CardDatatable GetCardDatatable(int page, int itemsPerPage, string sortBy, bool reverse, string type, string difficulty, string searchValue);
		void CreateCard(CardViewModel card);
		void UpdateCard(CardViewModel card);
		void DeleteCard(string id);
		void SaveCard();
	}

	public class CardService : ICardService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly ICardRepository _cardRepository;
		private readonly ICardGroupRepository _cardGroupRepository;

		public CardService(IUnitOfWork unitOfWork, ICardRepository cardRepository, ICardGroupRepository cardGroupRepository)
		{
			this._cardRepository = cardRepository;
			this._cardGroupRepository = cardGroupRepository;
			this._unitOfWork = unitOfWork;
		}

		public CardViewModel GetCardInfo(string id)
		{
			var card = _cardRepository.Query(p => p.Id == id).FirstOrDefault();
			if (card != null)
			{
				var cardGroup = _cardGroupRepository.Query(p => p.Id == card.GroupId).FirstOrDefault();
				var vmCard = Mapper.Map<Card, CardViewModel>(card);
				if (cardGroup != null)
				{
					vmCard.GroupName = cardGroup.Name;
				}
				return vmCard;
			}
			return null;
		}

		public IEnumerable<CardViewModel> GetAutoSuggestCard(string value)
		{
			var cards = _cardRepository.Query(p => p.Question1.ToLower().Contains(value) ||
													p.Question2.ToLower().Contains(value) ||
													p.Question3.ToLower().Contains(value));
			if (cards != null)
			{
				var destination = Mapper.Map<IEnumerable<Card>, IEnumerable<CardViewModel>>(cards);
				return destination;
			}
			return null;
		}

		public CardDatatable GetCardDatatable(int page, int itemsPerPage, string sortBy, bool reverse, string type, string difficulty, string searchValue)
		{
			//var cards = _cardRepository.GetAllQueryable();
			var cards = from p in _cardRepository.GetAllQueryable()
						join q in _cardGroupRepository.GetAllQueryable() on p.GroupId equals q.Id into pq
						from q in pq.DefaultIfEmpty()
						select new CardViewModel()
						{
							Id = p.Id,
							GroupId = p.GroupId,
							GroupName = q.Name,
							Question1 = p.Question1,
							Question2 = p.Question2,
							Question3 = p.Question3,
							CreatedDate = p.CreatedDate,
							UpdatedDate = p.UpdatedDate,
							Type = p.Type,
							Difficulty = p.Difficulty
						};

			// searching
			searchValue = string.IsNullOrWhiteSpace(searchValue) ? "" : searchValue.ToLower();
			type = string.IsNullOrWhiteSpace(type) ? "" : type.ToLower();
			cards = cards.Where(p => (searchValue == "" ||
										p.Question1.ToLower().Contains(searchValue) ||
										p.Question2.ToLower().Contains(searchValue) ||
										p.Question3.ToLower().Contains(searchValue)) &
										(type == "" || p.Type == type) &
										(string.IsNullOrEmpty(difficulty) || p.Difficulty == difficulty));
			//if (!string.IsNullOrWhiteSpace(searchValue))
			//{
			//    searchValue = searchValue.ToLower();
			//    cards = cards.Where(p => p.Type == searchValue);
			//}

			// sorting (done with the System.Linq.Dynamic library available on NuGet)
			var cardsOrdered = cards.OrderBy(sortBy + (reverse ? " descending" : ""));

			// paging
			var cardsPaged = cardsOrdered.Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();

			//var destination = Mapper.Map<List<Card>, List<CardViewModel>>(cardsPaged);
			var cardDatatable = new CardDatatable()
			{
				Data = cardsPaged,
				Total = cards.Count()
			};
			return cardDatatable;
		}

		public void CreateCard(CardViewModel vmCard)
		{
			//for testing
			//     for (var i = 0; i < 100; i++)
			//     {
			//      var newCard = new Card();
			//newCard.Id = Guid.NewGuid().ToString();
			//      newCard.Question1 = "Question1_" + i;
			//newCard.Question2 = "Question2_" + i;
			//newCard.Question3 = "Question2_" + i;
			//      newCard.Type = "A";
			//newCard.Difficulty = "2";
			//_cardRepository.Add(newCard);
			//SaveCard();
			//     }

			var card = Mapper.Map<CardViewModel, Card>(vmCard);
			card.Id = Guid.NewGuid().ToString();
			_cardRepository.Add(card);
			SaveCard();
		}

		public void UpdateCard(CardViewModel vmCard)
		{
			var card = Mapper.Map<CardViewModel, Card>(vmCard);
			card.UpdatedDate = DateTime.Now;
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
