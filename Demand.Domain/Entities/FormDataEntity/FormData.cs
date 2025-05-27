using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demand.Domain.Entities.FormDataEntity
{
    public class FormData
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public Services Services { get; set; }
    }

    public class Services
    {
        public bool TurnkeyExhibitions { get; set; }
        public bool ContentDigitalArt { get; set; }
        public bool OperationsTechnology { get; set; }
        public bool Other { get; set; }
    }
}
