using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Root.Data.Infrastructure;

namespace Service.Services
{
	public interface IPlayerService
	{
		void SavePlayer();
	}

	public class PlayerService: IPlayerService
	{
		private readonly IUnitOfWork _unitOfWork;
		public PlayerService(IUnitOfWork unitOfWork)
		{
			this._unitOfWork = unitOfWork;
		}
		public void SavePlayer()
		{
			_unitOfWork.Commit();
		}
	}
}
