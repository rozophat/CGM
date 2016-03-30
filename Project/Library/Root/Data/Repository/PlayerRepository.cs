using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Root.Data.Infrastructure;
using Root.Models;

namespace Root.Data.Repository
{
	public class PlayerRepository : RepositoryBase<Player>, IPlayerRepository
	{
		public PlayerRepository(IDatabaseFactory databaseFactory)
			: base(databaseFactory)
		{

		}
	}
	public interface IPlayerRepository : IRepository<Player>
	{
	}
}
