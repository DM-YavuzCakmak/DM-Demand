using Demand.Domain.Entities.InvoiceEntity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demand.Domain.NebimModels
{
    public class IncomingEInvoiceHeaderModel
    {
        public Guid InvoiceHeaderID { get; set; }
        public string FromIntegratorUUID { get; set; }
        public string EInvoiceNumber { get; set; }
        public string ProcessCode { get; set; }
        public bool IsReturn { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime InvoiceTime { get; set; }
        public string Description { get; set; }
        public int ConfirmationStatusCode { get; set; }
        public int CurrAccTypeCode { get; set; }
        public string CurrAccCode { get; set; }
        public decimal PayableAmount { get; set; }
        public string CurrAccDescription { get; set; }
        public string TaxNumber { get; set; }
        public string IdentityNum { get; set; }
        public string TaxExemptionDescription { get; set; }
        public double? StoppageRate { get; set; }
        public string DocCurrencyCode { get; set; }
        public double? TDisRate1 { get; set; }
        public double? TDisRate2 { get; set; }
        public double? TDisRate3 { get; set; }
        public double? TDisRate4 { get; set; }
        public double? TDisRate5 { get; set; }
        public bool IsExpenseSlip { get; set; }
        public string CompanyCode { get; set; }
        public string OfficeCode { get; set; }
        public string EInvoiceAliasCode { get; set; }

        [NotMapped]
        public string CompanyName { get; set; }
        [NotMapped]
        public int Status { get; set; } = -1;
        [NotMapped]
        public InvoiceDetailEntity? InvoiceDetailEntity { get; set; }
    }
}
