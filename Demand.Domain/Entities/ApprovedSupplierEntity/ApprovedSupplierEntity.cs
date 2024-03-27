using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demand.Domain.Entities.ApprovedSupplierEntity
{

    [Table("ApprovedSuppliers")]
    public class ApprovedSupplierEntity:BaseEntity
    {
        public long? ApprovedBy { get; set; }
        public string SupplierName { get; set; }
        public string SupplierPhone { get; set; }
        public string SupplierAddress { get; set; }
        public string SupplierContact { get; set; }

    }
}
