using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demand.Domain.NebimModels
{
    public class NebimSubCategoryModel
    {
        public string? ProductHierarchyLevel01Code { get; set; }
        public string? ProductHierarchyLevel02Code { get; set; }
        public string? ProductHierarchyLevel02Description { get; set; }
        public string? CompanyBrandCode { get; set; }
        public string? CompanyCode { get; set; }
    }
}
