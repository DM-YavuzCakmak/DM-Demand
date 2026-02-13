using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demand.Domain.NebimModels
{
    public class NebimProductModel
    {
        public string? CompanyName { get; set; }
        public string? ProductHierarchyLevel01Code { get; set; }
        public string? ProductHierarchyLevel01Description { get; set; }
        public string? ProductHierarchyLevel02Code { get; set; }
        public string? ProductHierarchyLevel02Description { get; set; }
        public string? CompanyBrandCode { get; set; }
        public string? CompanyCode { get; set; }
        public string? ProductCode { get; set; }
        public string? ProductDescription { get; set; }
        public string? ItemTaxGrCode { get; set; }
        public string? UnitOfMeasureCode { get; set; }
    }
}
