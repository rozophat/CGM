using AutoMapper;
using Root.Data.Infrastructure;
using Root.Data.Repository;
using Root.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;
using Website.ViewModels.Card;
using Website.ViewModels.CardGroup;

namespace Service.Services
{
    public interface ICardGroupService
    {
        CardGroupViewModel GetCardGroupInfo(string id);
        CardGroupDatatable GetCardGroupDatatable(int page, int itemsPerPage, string sortBy, bool reverse, string searchValue);
        CardDatatable GetCardDatatable(int page, int itemsPerPage, string sortBy, bool reverse, string id, string type, string difficulty, string searchValue);
        IEnumerable<CardGroupViewModel> GetAutoSuggestCardGroup(string value);
        void CreateCardGroup(CardGroupViewModel cardGroup);
        void UpdateCardGroup(CardGroupViewModel cardGroup);
        void DeleteCardGroup(string id);
        void SaveCardGroup();
    }

    public class CardGroupService : ICardGroupService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICardRepository _cardRepository;
        private readonly ICardGroupRepository _cardGroupRepository;

        public CardGroupService(IUnitOfWork unitOfWork, ICardGroupRepository cardGroupRepository, ICardRepository cardRepository)
        {
            this._cardGroupRepository = cardGroupRepository;
            this._cardRepository = cardRepository;
            this._unitOfWork = unitOfWork;
        }

        public CardGroupViewModel GetCardGroupInfo(string id)
        {
            var cardGroup = _cardGroupRepository.Query(p => p.Id == id).FirstOrDefault();
            if (cardGroup != null)
            {
                var vmCardGroup = Mapper.Map<CardGroup, CardGroupViewModel>(cardGroup);
                return vmCardGroup;
            }
            return null;
        }

        public CardGroupDatatable GetCardGroupDatatable(int page, int itemsPerPage, string sortBy, bool reverse, string searchValue)
        {
            var cardGroups = _cardGroupRepository.GetAllQueryable();
            // searching
            if (!string.IsNullOrWhiteSpace(searchValue))
            {
                searchValue = searchValue.ToLower();
                cardGroups = cardGroups.Where(p => p.Name.ToLower().Contains(searchValue) ||
                                                p.Type == searchValue ||
                                                p.AppleProductCode.Equals(searchValue.ToLower()) ||
                                                p.GoogleProductCode.Equals(searchValue.ToLower()));
            }

            // sorting (done with the System.Linq.Dynamic library available on NuGet)
            var cardGroupsOrdered = cardGroups.OrderBy(sortBy + (reverse ? " descending" : ""));

            // paging
            var cardGroupsPaged = cardGroupsOrdered.Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();

            var destination = Mapper.Map<List<CardGroup>, List<CardGroupViewModel>>(cardGroupsPaged);
            var cardGroupDatatable = new CardGroupDatatable()
            {
                Data = destination,
                Total = cardGroups.Count()
            };
            return cardGroupDatatable;
        }

        public CardDatatable GetCardDatatable(int page, int itemsPerPage, string sortBy, bool reverse, string id, string type, string difficulty, string searchValue)
        {
            var cards = from p in _cardRepository.GetAllQueryable()
                        where p.GroupId == id
                        select new CardViewModel()
                        {
                            Id = id,
                            GroupId = p.GroupId,
                            Question1 = p.Question1,
                            Question2 = p.Question2,
                            Question3 = p.Question3,
                            Type = p.Type,
                            Difficulty = p.Difficulty,
                            CreatedDate = p.CreatedDate,
                            UpdatedDate = p.UpdatedDate
                        };
            // searching
            // searching
            searchValue = string.IsNullOrWhiteSpace(searchValue) ? "" : searchValue.ToLower();
            type = string.IsNullOrWhiteSpace(type) ? "" : type.ToLower();
            cards = cards.Where(p => (searchValue == "" || 
                                        p.Question1.ToLower().Contains(searchValue) ||
                                        p.Question2.ToLower().Contains(searchValue) ||
                                        p.Question3.ToLower().Contains(searchValue)) &
                                        (type == "" || p.Type == type) &
                                        (string.IsNullOrEmpty(difficulty) || p.Difficulty == difficulty));


            // sorting (done with the System.Linq.Dynamic library available on NuGet)
            var cardOrdered = cards.OrderBy(sortBy + (reverse ? " descending" : ""));

            // paging
            var cardPaged = cardOrdered.Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();

            var cardDatatable = new CardDatatable()
            {
                Data = cardPaged,
                Total = cards.Count()
            };
            return cardDatatable;
        }

        public IEnumerable<CardGroupViewModel> GetAutoSuggestCardGroup(string value)
        {
            var cardGroups = _cardGroupRepository.Query(p => p.Name.Contains(value));
            if (cardGroups != null)
            {
                var destination = Mapper.Map<IEnumerable<CardGroup>, IEnumerable<CardGroupViewModel>>(cardGroups);
                return destination;
            }
            return null;
        }

        public void CreateCardGroup(CardGroupViewModel vmCardGroup)
        {
            var cardGroups = Mapper.Map<CardGroupViewModel, CardGroup>(vmCardGroup);
            cardGroups.Id = Guid.NewGuid().ToString();
            _cardGroupRepository.Add(cardGroups);
            SaveCardGroup();
        }

        public void UpdateCardGroup(CardGroupViewModel vmCardGroup)
        {
            var cardGroup = Mapper.Map<CardGroupViewModel, CardGroup>(vmCardGroup);
            _cardGroupRepository.Update(cardGroup);
            SaveCardGroup();
        }

        public void DeleteCardGroup(string id)
        {
            _cardGroupRepository.Delete(p => p.Id == id);
            SaveCardGroup();
        }

        public void SaveCardGroup()
        {
            _unitOfWork.Commit();
        }

       
    }
}
