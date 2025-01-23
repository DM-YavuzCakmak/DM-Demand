using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demand.Domain.DTO
{
    public class DemandReminderDto
    {
        public long DemandId { get; set; }
        public long ManagerId { get; set; }
        public DateTime RelevantDate { get; set; }
    }
}
