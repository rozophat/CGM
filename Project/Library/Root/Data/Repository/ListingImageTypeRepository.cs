﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Root.Data.Infrastructure;
using Root.Models;

namespace Root.Data.Repository
{
	public class ListingImageTypeRepository : RepositoryBase<ListingImageType>, IListingImageTypeRepository
	{
		public ListingImageTypeRepository(IDatabaseFactory databaseFactory)
			: base(databaseFactory)
		{

		}
	}
	public interface IListingImageTypeRepository : IRepository<ListingImageType>
	{
	}
}
