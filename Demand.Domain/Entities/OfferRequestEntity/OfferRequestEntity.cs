using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demand.Domain.Entities.OfferRequestEntity
{
    [Table("OfferRequest")]
    public class OfferRequestEntity:BaseEntity
    {
        public long RequestInfoId { get; set; }
        public long DemandOfferId { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? TotalPrice { get; set; }
        public int? Status { get; set; }
    }
}