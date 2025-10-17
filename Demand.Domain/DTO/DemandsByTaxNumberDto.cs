using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demand.Domain.DTO
{
    public class DemandsByTaxNumberDto
    {
        public long DemandId { get; set; }
        public required string DemandTitle { get; set; }
        public required string DepartmentName { get; set; }
        public long ProviderId { get; set; }
        public required string ProviderName { get; set; }
        public required string TaxNumber { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal RemainingPrice { get; set; }
        public decimal? MatchingPrice { get; set; }
        public required string CurrencySymbol { get; set; }
        public decimal ExchangeRate { get; set; }
    }
}
