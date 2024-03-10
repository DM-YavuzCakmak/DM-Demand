using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demand.Domain.ViewModels
{
    public class UpdateDemandViewModel
    {
        public long DemandId { get; set; }
        public long CompanyId { get; set; }
        public long CompanyLocationId { get; set; }
        public long DepartmentId { get; set; }
        public string Description { get; set; }
    }
}
