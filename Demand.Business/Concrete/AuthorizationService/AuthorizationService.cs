using Demand.Business.Abstract.AuthorizationService;
using Demand.Core.Utilities.Results.Abstract;
using Demand.Core.Utilities.Results.Concrete;
using Demand.Domain.Entities.Company;
using Demand.Domain.Entities.Personnel;
using Demand.Domain.ViewModels;
using Demand.Infrastructure.DataAccess.Abstract.Personnel;

namespace Demand.Business.Concrete.AuthorizationService;

public class AuthorizationService : IAuthorizationService
{
    private readonly IPersonnelRepository _personnelRepository;

    public AuthorizationService(IPersonnelRepository personnelRepository)
    {
        _personnelRepository = personnelRepository;
    }

    public IDataResult<PersonnelEntity> Login(LoginViewModel loginViewModel)
    {
        PersonnelEntity? personnelEntity = _personnelRepository.Get(x => x.Email == loginViewModel.UserEmail);
        if (personnelEntity != null)
        {
            return new SuccessDataResult<PersonnelEntity>(personnelEntity);
        }
        else
        {
            return new DataResult<PersonnelEntity>(null, false);
        }

    }
}
