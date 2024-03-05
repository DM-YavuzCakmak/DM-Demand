using Demand.Business.Abstract.AuthorizationService;
using Demand.Infrastructure.DataAccess.Abstract.Personnel;

namespace Demand.Business.Concrete.AuthorizationService;

public class AuthorizationService : IAuthorizationService
{
    private readonly IPersonnelRepository _personnelRepository;

    public AuthorizationService(IPersonnelRepository personnelRepository)
    {
        _personnelRepository = personnelRepository;
    }

    public void Login()
    {
        var aa = _personnelRepository.GetAll();
        //throw new NotImplementedException();
    }
}
