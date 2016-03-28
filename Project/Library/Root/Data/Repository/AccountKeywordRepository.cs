using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Root.Data.Infrastructure;
using Root.Models;

namespace Root.Data.Repository
{
	public class AccountKeywordRepository : RepositoryBase<AccountKeyword>, IAccountKeywordRepository
	{
		public AccountKeywordRepository(IDatabaseFactory databaseFactory)
			: base(databaseFactory)
		{

		}
	}
	public interface IAccountKeywordRepository : IRepository<AccountKeyword>
	{
	}
}
