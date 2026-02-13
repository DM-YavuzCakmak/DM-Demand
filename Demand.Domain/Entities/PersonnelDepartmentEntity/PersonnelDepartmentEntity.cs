using System.ComponentModel.DataAnnotations.Schema;

namespace Demand.Domain.Entities.PersonnelDepartmentEntity
{
    [Table("PersonnelDepartment")]
    public class PersonnelDepartmentEntity : BaseEntity
    {
        public long PersonnelId { get; set; }
        public long DepartmentId { get; set; }
        public int ApproveLevel { get; set; }
        public bool IsJustInformation { get; set; }
    }
}
