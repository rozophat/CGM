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
using Website.ViewModels.CardGroup;

namespace Service.Services
{
    public interface ICardGroupService
    {
        CardGroupViewModel GetCardGroupInfo(string id);
        CardGroupDatatable GetCardGroupDatatable(int page, int itemsPerPage, string sortBy, bool reverse, string searchValue);
        void CreateCardGroup(CardGroupViewModel cardGroup);
        void UpdateCardGroup(CardGroupViewModel cardGroup);
        void DeleteCardGroup(string id);
        void SaveCardGroup();
    }

    public class CardGroupService : ICardGroupService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICardGroupRepository _cardGroupRepository;

        public CardGroupService(IUnitOfWork unitOfWork, ICardGroupRepository cardGroupRepository)
        {
            this._cardGroupRepository = cardGroupRepository;
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
