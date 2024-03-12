using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demand.Domain.Entities.DemandOfferEntity
{
    [Table("DemandOffers")]
    public class DemandOfferEntity:BaseEntity
    {
        public long DemandId { get; set; }
        public long CurrencyTypeId { get; set; }
        public long RequestInfoId { get; set; }
        public decimal? TotalPrice { get; set; }
        public int? Status { get; set; }
    }
}
