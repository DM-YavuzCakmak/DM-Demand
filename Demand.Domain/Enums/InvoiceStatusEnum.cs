namespace Demand.Domain.Enums
{
    public enum InvoiceStatusEnum
    {
        New = 0,
        Pending = 1,
        FirstLevelApproved = 2,
        SecondLevelApproved = 3,
        Approved = 4,
        Rejected = 5,
    }
}
