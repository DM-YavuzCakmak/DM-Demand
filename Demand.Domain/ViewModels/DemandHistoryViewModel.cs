using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demand.Domain.ViewModels
{
    public class DemandHistoryViewModel
    {
        public string? PersonnelFullName { get; set; }
        public string? Stage { get; set; }
        public DateTime? Date { get; set; }
        public string? Status { get; set; }
    }
}
