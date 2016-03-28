using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Root.Data.Infrastructure;
using Root.Models;

namespace Root.Data.Repository
{
	public class BannerImageRepository : RepositoryBase<BannerImage>, IBannerImageRepository
	{
		public BannerImageRepository(IDatabaseFactory databaseFactory)
			: base(databaseFactory)
		{

		}
	}
	public interface IBannerImageRepository : IRepository<BannerImage>
	{
	}
}
