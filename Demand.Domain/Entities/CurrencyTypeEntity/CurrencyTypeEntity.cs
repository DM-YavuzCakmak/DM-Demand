using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demand.Domain.Entities.CurrencyTypeEntity
{
    [Table("CurrencyType")]
    public class CurrencyTypeEntity:BaseEntity
    {
        public string CurrencyType { get; set; }
        public string Symbol { get; set; }

    }
}
