﻿using Demand.Core.DataAccess.EntityFramework;
using Demand.Domain.Entities.CompanyLocation;

namespace Demand.Infrastructure.DataAccess.Abstract;
public interface ICompanyLocationRepository : IEntityRepository<CompanyLocation>
{
}
