using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demand.Domain.Entities.DepartmentEntity
{
    [Table("Department")]
    public class DepartmentEntity : BaseEntity
    {


        [MaxLength(255)]
        public string Name { get; set; }

    }
}