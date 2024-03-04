
namespace Demand.Domain.Entities.Personnel;

public class Personnel : BaseEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public long ParentId { get; set; }
}
