namespace Demand.Domain.NebimModels
{
    public class IncomingEInvoiceLineModel
    {
        // ===== Invoice Header =====
        public Guid InvoiceHeaderID { get; set; }              // DFF5B41D-...
        public string EInvoiceNumber { get; set; }             // ZE22025000001905
        public string ProcessCode { get; set; }                // BP
        public bool IsReturn { get; set; }                     // 0/1
        public DateTime InvoiceDate { get; set; }              // 2025-08-12
        public DateTime InvoiceTime { get; set; }              // 09:35:00
        public string Description { get; set; }                // 12.08.2025 tarihli ...
        public int ConfirmationStatusCode { get; set; }        // 0
        public int CurrAccTypeCode { get; set; }               // 1
        public string CurrAccCode { get; set; }                // boş geldi (nullable string)
        public string CurrAccDescription { get; set; }         // ZE-ME GIDA TEKSTİL ...
        public string TaxNumber { get; set; }                  // 9970658373
        public string IdentityNum { get; set; }                // boş
        public int TaxTypeCode { get; set; }                   // 0
        public string WithHoldingTaxTypeCode { get; set; }     // boş
        public string DovCode { get; set; }                    // boş
        public string TaxExemptionDescription { get; set; }    // boş
        public double? StoppageRate { get; set; }              // 0.0
        public string DocCurrencyCode { get; set; }            // TRY
        public decimal ExchangeRate { get; set; }              // 1.0
        public decimal? PayableAmount { get; set; }            // boş
        public double? TDisRate1 { get; set; }
        public double? TDisRate2 { get; set; }
        public double? TDisRate3 { get; set; }
        public double? TDisRate4 { get; set; }
        public double? TDisRate5 { get; set; }
        public bool IsExpenseSlip { get; set; }                // 0
        public string CompanyCode { get; set; }                // 1
        public string OfficeCode { get; set; }                 // boş
        public string EInvoiceAliasCode { get; set; }          // boş

        // ===== Invoice Line =====
        public int SortOrder { get; set; }                     // 56, 57
        public int ItemTypeCode { get; set; }                  // 1
        public string ItemCode { get; set; }                   // boş
        public string ColorCode { get; set; }                  // boş
        public string ItemDim1Code { get; set; }               // boş
        public string ItemDim2Code { get; set; }               // boş
        public string ItemDim3Code { get; set; }               // boş
        public string ItemName { get; set; }                   // COCA COLA ZERO..., NESCAFE...
        public string UnitOfMeasureCode { get; set; }          // C62
        public decimal Qty1 { get; set; }                      // 1.0, 5.0
        public string UsedBarcode { get; set; }                // N05635, N07459
        public decimal VatRate { get; set; }                   // 10.0
        public string PCTCode { get; set; }                    // boş
        public decimal? PCTRate { get; set; }                  // null
        public decimal? LDisRate1 { get; set; }
        public decimal? LDisRate2 { get; set; }
        public decimal? LDisRate3 { get; set; }
        public decimal? LDisRate4 { get; set; }
        public decimal? LDisRate5 { get; set; }
        public string PriceCurrencyCode { get; set; }          // TRY
        public decimal PriceExchangeRate { get; set; }         // 1.0
        public string LineDescription { get; set; }            // barcode (5000112665307...)
        public string ManufacturersItemIdentification { get; set; } // tekrar barcode gibi

        // ===== Invoice Line Currency =====
        public decimal Price { get; set; }                     // 227.27, 54.546
        public decimal Amount { get; set; }                    // 227.27, 272.73
        public decimal LDiscount1 { get; set; }
        public decimal LDiscount2 { get; set; }
        public decimal LDiscount3 { get; set; }
        public decimal LDiscount4 { get; set; }
        public decimal LDiscount5 { get; set; }
        public decimal TDiscount1 { get; set; }
        public decimal TDiscount2 { get; set; }
        public decimal TDiscount3 { get; set; }
        public decimal TDiscount4 { get; set; }
        public decimal TDiscount5 { get; set; }
        public decimal TaxBase { get; set; }                   // 227.27, 272.73
        public decimal Pct { get; set; }                       // 0.0
        public decimal Vat { get; set; }                       // 22.73, 27.27
        public decimal NetAmount { get; set; }                 // 250.0, 300.0

        // ===== Expense Slip (örnekte boş geldi) =====
        public int? Expense_SortOrder { get; set; }
        public string Expense_GLAccCode { get; set; }
        public string Expense_LineDescription { get; set; }
        public string Expense_ItemDescription { get; set; }
        public decimal? Expense_TaxRate { get; set; }
        public decimal? Expense_Tax { get; set; }
        public decimal? Expense_Amount { get; set; }
        public decimal? Expense_TaxAmount { get; set; }
        public decimal? Expense_TaxAssessment { get; set; }
        public string Expense_PriceCurrencyCode { get; set; }
        public decimal? Expense_PriceExchangeRate { get; set; }

        // ===== Postal Address =====
        public string TaxOfficeName { get; set; }              // BODRUM VERGİ DAİRESİ
        public string TaxOfficeCode { get; set; }              // 048261
        public string FirstName { get; set; }                  // boş
        public string LastName { get; set; }                   // boş
        public string StreetName { get; set; }                 // 67. SOKAK
        public string BuildingNumber { get; set; }             // boş
        public string CitySubdivisionName { get; set; }        // GÖLKÖY MAHALLESİ / BODRUM
        public string CityName { get; set; }                   // MUĞLA
        public string CityCode { get; set; }                   // TR.48
        public string PostalZone { get; set; }                 // 48400
        public string CountryName { get; set; }                // TÜRKİYE
        public string CountryCode { get; set; }                // TR

        // ===== Additional Info =====
        public string OrderInfo { get; set; }
        public string ShipmentInfo { get; set; }
        public string CostCenterCode { get; set; }

    }
}
