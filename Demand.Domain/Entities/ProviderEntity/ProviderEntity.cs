using System.ComponentModel.DataAnnotations.Schema;

namespace Demand.Domain.Entities.ProviderEntity
{
    [Table("Provider")]
    public class ProviderEntity : BaseEntity
    {
        public string? Name { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? TaxNumber { get; set; }
        public string? NationalIdNumber { get; set; }
        public string? Email { get; set; }
        public string? TaxOfficeCode { get; set; }
        public bool IsApproved { get; set; } = false;
        public int? CityId { get; set; }
        public int? DistrictId { get; set; }

        [NotMapped]
        public string? CityName { get; set; }
        [NotMapped]
        public string? DistrictName { get; set; }
        [NotMapped]
        public string? TaxOfficeName { get; set; }
    }


}
