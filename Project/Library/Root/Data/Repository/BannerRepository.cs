using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Root.Data.Infrastructure;
using Root.Models;

namespace Root.Data.Repository
{
	public class BannerRepository : RepositoryBase<Banner>, IBannerRepository
	{
		public BannerRepository(IDatabaseFactory databaseFactory)
			: base(databaseFactory)
		{

		}
	}
	public interface IBannerRepository : IRepository<Banner>
	{
	}
}
