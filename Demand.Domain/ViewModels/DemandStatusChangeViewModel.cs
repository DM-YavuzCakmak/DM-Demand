namespace Demand.Domain.ViewModels
{
    public class DemandStatusChangeViewModel
    {
        public long DemandId { get; set; }
        public long? DemandOfferId { get; set; }
        public int Status { get; set; }
        public string? Description { get; set; }
    }
}
