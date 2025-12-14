namespace Demand.Domain.NebimModels
{
    public class NebimWarehouseModel
    {
        public required string CompanyName { get; set; }
        public required string OfficeCode { get; set; }
        public required string OfficeDescription { get; set; }
        public required string WarehouseCode { get; set; }
        public required string WarehouseDescription { get; set; }
    }
}
