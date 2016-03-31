using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Root.Data.Infrastructure;

namespace Service.Services
{
	public interface ICardService
	{
		void SaveCard();
	}

	public class CardService : ICardService
	{
		private readonly IUnitOfWork _unitOfWork;
		public CardService(IUnitOfWork unitOfWork)
		{
			this._unitOfWork = unitOfWork;
		}
		public void SaveCard()
		{
			_unitOfWork.Commit();
		}
	}
}
