using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Root.Data.Infrastructure;
using Root.Models;

namespace Root.Data.Repository
{
	public class CardGroupRepository : RepositoryBase<CardGroup>, ICardGroupRepository
	{
		public CardGroupRepository(IDatabaseFactory databaseFactory)
			: base(databaseFactory)
		{

		}
	}
	public interface ICardGroupRepository : IRepository<CardGroup>
	{
	}
}
