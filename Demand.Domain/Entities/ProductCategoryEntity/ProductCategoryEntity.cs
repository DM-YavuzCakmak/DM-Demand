using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demand.Domain.Entities.ProductCategoryEntity
{
    [Table("ProductCategory")]
    public class ProductCategoryEntity:BaseEntity
    {
        public string ProductCategoryName { get; set; }
        public bool? IntegratedWithAcc { get; set; }
    }
}
