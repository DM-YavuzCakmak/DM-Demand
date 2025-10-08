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
        public decimal? TotalPrice { get; set; }
        public int? Status { get; set; }
        public int? UnitManager { get; set; }
        public DateTime? DeadlineDate { get; set; }
        public DateTime? MaturityDate { get; set; }
        public int? PaymentType { get; set; }
        public int? InstallmentPayment { get; set; }
        public int? PartialPayment { get; set; }
        public long? SupplierId { get; set; }
        public decimal? ExchangeRate { get; set; }
    }
}