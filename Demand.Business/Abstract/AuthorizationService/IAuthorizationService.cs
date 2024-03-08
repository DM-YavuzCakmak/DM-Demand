
using Demand.Core.Utilities.Results.Abstract;
using Demand.Domain.Entities.Company;
using Demand.Domain.Entities.Personnel;
using Demand.Domain.ViewModels;

namespace Demand.Business.Abstract.AuthorizationService;

public interface IAuthorizationService
{
     IDataResult<PersonnelEntity> Login(LoginViewModel loginViewModel);
}
