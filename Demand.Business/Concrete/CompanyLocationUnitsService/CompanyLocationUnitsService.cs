using Demand.Business.Abstract.CompanyLocationUnitsService;
using Demand.Core.Utilities.Results.Abstract;
using Demand.Core.Utilities.Results.Concrete;
using Demand.Domain.Entities.CompanyLocationUnitsEntity;
using Demand.Infrastructure.DataAccess.Abstract.CompanyLocationUnits;
using Demand.Infrastructure.DataAccess.Abstract.ICompanyLocationRepository;
using Demand.Infrastructure.DataAccess.Concrete.EntityFramework.CompanyLocationRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demand.Business.Concrete.CompanyLocationUnitsService
{
    public class CompanyLocationUnitsService : ICompanyLocationUnitsService
    {
        private readonly ICompanyLocationUnitsRepository _companyLocationUnitsRepository;
        public CompanyLocationUnitsService(ICompanyLocationUnitsRepository companyLocationUnitsRepository)
        {
            _companyLocationUnitsRepository = companyLocationUnitsRepository;
        }

        public IDataResult<IList<CompanyLocationUnitsEntity>> GetAll()
        {
            return new SuccessDataResult<IList<CompanyLocationUnitsEntity>>(_companyLocationUnitsRepository.GetAll());
        }

        public IDataResult<CompanyLocationUnitsEntity> GetById(long id)
        {
            return new SuccessDataResult<CompanyLocationUnitsEntity>(_companyLocationUnitsRepository.Get(x => x.Id == id));
        }

        public IDataResult<List<CompanyLocationUnitsEntity>> GetLocationUnitsByLocationId(long locationId)
        {
            var locationUnits = _companyLocationUnitsRepository.GetList(x => x.LocationId == locationId).ToList();

            return new SuccessDataResult<List<CompanyLocationUnitsEntity>>(locationUnits);
        }

        IDataResult<List<CompanyLocationUnitsEntity>> ICompanyLocationUnitsService.GetList()
        {
            return new SuccessDataResult<List<CompanyLocationUnitsEntity>>(_companyLocationUnitsRepository.GetList().ToList());
        }
    }
}
