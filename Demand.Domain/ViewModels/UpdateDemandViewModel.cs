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
        public string? Offer1CompanyName { get; set; }
        public long? Offer1CompanyId { get; set; }
        public string? Offer1CompanyPhone { get; set; }
        public string? Offer1CompanyAddress { get; set; }
        public int? Offer1CurrencyType { get; set; }
        public decimal? Offer1Amount { get; set; }
        public string? Offer1Material { get; set; }
        public decimal? Offer1Price { get; set; }
        public decimal? Offer1TotalPrice { get; set; }

        public string? Offer2CompanyName { get; set; }
        public long? Offer2CompanyId { get; set; }

        public string? Offer2CompanyPhone { get; set; }
        public string? Offer2CompanyAddress { get; set; }
        public int? Offer2CurrencyType { get; set; }
        public decimal? Offer2Amount { get; set; }
        public string? Offer2Material { get; set; }
        public decimal? Offer2Price { get; set; }
        public decimal? Offer2TotalPrice { get; set; }

        public string? Offer3CompanyName { get; set; }
        public long? Offer3CompanyId { get; set; }

        public string? Offer3CompanyPhone { get; set; }
        public string? Offer3CompanyAddress { get; set; }
        public int? Offer3CurrencyType { get; set; }
        public decimal? Offer3Amount { get; set; }
        public string? Offer3Material { get; set; }
        public decimal? Offer3Price { get; set; }
        public decimal? Offer3TotalPrice { get; set; }
        public bool IsProvider1Registered { get; set; }
        public bool IsProvider2Registered { get; set; }
        public bool IsProvider3Registered { get; set; }
    }
}
