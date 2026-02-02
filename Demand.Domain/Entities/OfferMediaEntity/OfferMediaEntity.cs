using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Demand.Domain.Entities.OfferMediaEntity
{
    [Table("OfferMedia")]
    public class OfferMediaEntity : BaseEntity
    {
        [Required]
        public long OfferId { get; set; }
        [MaxLength]
        public string Path { get; set; }
        public string? FileName { get; set; }
        public long? DemandId { get; set; }

        [ForeignKey("OfferId")]
        public virtual DemandOfferEntity.DemandOfferEntity DemandOffer { get; set; }
    }
}
