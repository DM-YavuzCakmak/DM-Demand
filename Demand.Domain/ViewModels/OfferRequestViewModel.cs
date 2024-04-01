using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demand.Domain.ViewModels
{
    public class OfferRequestViewModel
    {
        public long DemandId { get; set; }
        public int? ProductCategoryId { get; set; }
        public int? ProductSubCategoryId { get; set; }
        public string ProductName { get; set; }
        public int? Quantity { get; set; }
        public string Unit { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
