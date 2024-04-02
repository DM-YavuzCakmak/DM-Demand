using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demand.Domain.NebimModels
{
    public class NebimProductModel
    {
        public string? ProductHierarchyLevel01Code { get; set; }
        public string? ProductHierarchyLevel02Code { get; set; }
        public string? ProductCode { get; set; }
        public string? ProductDescription { get; set; }
    }
}
