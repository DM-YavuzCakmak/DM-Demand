using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demand.Domain.ViewModels
{
    public class DemandStatusChangeViewModel
    {
        public long DemandId { get; set; }
        public int Status { get; set; }
        public string? Description { get; set; }
    }
}
