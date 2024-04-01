using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demand.Domain.NebimProductModel
{
    public class NebimProductsInfoModel
    {
        public string? ProductCode { get; set; }
        public string? productDescription { get; set; }
        public string? productHierarchyLevel01 { get; set; }
        public string? productHierarchyLevel02 { get; set; }
    }
}
