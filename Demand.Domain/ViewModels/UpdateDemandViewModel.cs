using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demand.Domain.ViewModels
{
    public class UpdateDemandViewModel
    {
        public long? DemandId { get; set; }
        public long? CompanyId { get; set; }
        public string? DemandTitle { get; set; }
        public long? CompanyLocationId { get; set; }
        public long? DepartmentId { get; set; }
        public string? Description { get; set; }

        // Teklif bilgileri
        public string? OfferCompanyName { get; set; }
        public long? OfferCompanyId { get; set; }
        public string? OfferCompanyPhone { get; set; }
        public string? OfferCompanyAddress { get; set; }
        public int? OfferCurrencyType { get; set; }
        public decimal? OfferAmount { get; set; }
        public string? OfferMaterial { get; set; }
        public decimal? OfferPrice { get; set; }
        public decimal? OfferTotalPrice { get; set; }
        public bool IsProvider1Registered { get; set; }
        public bool IsProvider2Registered { get; set; }
        public bool IsProvider3Registered { get; set; }
    }
}
