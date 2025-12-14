namespace Demand.Domain.NebimModels
{
    public class NebimExpenseModel
    {
        public required string ExpenseCode { get; set; }
        public required string ExpenseDescription { get; set; }
        public required string ItemTaxGrCode { get; set; }
        public required string ItemTaxGrDescription { get; set; }
        public bool IsBlocked { get; set; }
    }
}
