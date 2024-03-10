using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demand.Domain.ViewModels
{
    public class DemandViewModel
    {
        public long? CompanyId { get; set; }
        public long? DemandId { get; set; }
        public string? DemanderName { get; set; }
        public string? LocationName { get; set; }
        public DateTime DemandDate { get; set; }
        public int? Status{ get; set; }
        public long? CompanyLocationId { get; set; }
        public long? DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        public string? CompanyName { get; set; }
        public string? Description { get; set; }
        public DateTime? RequirementDate { get; set; }
        public bool? IsDeleted { get; set; } = false;
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long? CreatedAt { get; set; }
        public long? UpdatedAt { get; set; }
        public IFormFile? File1 { get; set; }
        public IFormFile? File2 { get; set; }
        public IFormFile? File3 { get; set; }
        public string? File1Path { get; set; }
        public byte[]? File2Path { get; set; }
        public byte[]? File3Path { get; set; }

    }
}
