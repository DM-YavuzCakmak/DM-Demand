using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Demand.Domain.Entities.DepartmentEntity
{
    [Table("Department")]
    public class DepartmentEntity : BaseEntity
    {


        [MaxLength(255)]
        public string Name { get; set; }
        public decimal? ThirdLevelInvoiceApproveLimit { get; set; }

    }
}