using Demand.Domain.Entities;
using Demand.Domain.NebimModels;
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
        #region DemandTableFields
        public long? CompanyId { get; set; }
        public long? DemandId { get; set; }
        public long? DemandOfferId { get; set; }
        public string? DemandTitle { get; set; }
        public string? DemanderName { get; set; }
        public string? LocationName { get; set; }
        public DateTime DemandDate { get; set; }
        public string? AprrovedDate { get; set; }
        public string?  WhoseTurn { get; set; }
        public string?  Yk { get; set; }
        public bool? isWhoPersonnel { get; set; }
        public int? Status{ get; set; }
        public long? CompanyLocationId { get; set; }
        public long? LocationUnitId { get; set; }
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
        #endregion
        #region DemandMediaTableFields
        public IFormFile? File1 { get; set; }
        public IFormFile? File2 { get; set; }
        public IFormFile? File3 { get; set; }
        public byte[]? File1Path { get; set; }
        public byte[]? File2Path { get; set; }
        public byte[]? File3Path { get; set; }
        #endregion
        #region RequestInfoTableFields
        public string? Material1 { get; set; }
        public int? Quantity1 { get; set; }
        public string? Unit1 { get; set; }
        public string? CurrencySymbol1 { get; set; }
        public string? Material2 { get; set; }
        public int? Quantity2 { get; set; }
        public string? Unit2 { get; set; }
        public string? CurrencySymbol2 { get; set; }
        public string? Material3 { get; set; }
        public int? Quantity3 { get; set; }
        public string? Unit3 { get; set; }
        public string? CurrencySymbol3 { get; set; }
        public string? ConfirmingNote { get; set; }
        public List<string>? RequestInfoId { get; set; }
        public List<string>? Type { get; set; }
        public List<string>? Category { get; set; }
        public List<string>? ProductName { get; set; }
        public List<string>? Product { get; set; }
        public List<string>? Subcategory { get; set; }
        public List<string>? Unit { get; set; }
        public List<string>? Quantity { get; set; }
        public List<string>? ProductDescription { get; set; }
        public List<string>? ProductCode { get; set; }
        public List<string>? Price { get; set; }
        public List<string>? TotalPrice { get; set; }
        public List<long>? OfferRequestId { get; set; }
        #endregion
        #region CurrencyTypeTableFields
        public int? CurrencyTypeId { get; set; }
        public string? CurrencyTypeText { get; set; }
        #endregion
        #region DemandOfferTableFields
        public List<DemandOfferViewModel>? DemandOffers { get; set; }
        public List<NebimCategoryModel>? NebimCategoryModels { get; set; }
        public List<NebimProductModel>? NebimProductModels { get; set; }
        public List<NebimSubCategoryModel>? NebimSubCategoryModels { get; set; }
        #endregion
        public List<RequestInfoViewModel>? requestInfoViewModels { get; set; }
        public List<IFormFile>? Files { get; set; }
        public string?  File1Name{ get; set; }
        public string?  File2Name{ get; set; }
        public string?  File3Name{ get; set; }
        #region ProviderTableFields
        #endregion
        public bool? isApprovedActive { get; set; }
        public decimal? ExchangeRate{ get; set; }
        public bool? isOppenOffer { get; set; } = false;
        public bool? isDemandOffer { get; set; } = false;
        public List<IFormFile>? ProformoFiles { get; set; }
        public string? ProformoFile1Name { get; set; }
        public string? ProformoFile2Name { get; set; }
        public string? ProformoFile3Name { get; set; }
        public byte[]? ProformoFile1Path { get; set; }
        public byte[]? ProformoFile2Path { get; set; }
        public byte[]? ProformoFile3Path { get; set; }
        public int? PaymentType { get; set; }
        public string? PaymentTypeText { get; set; }
        public int? PartialPayment  { get; set; }
        public string? PartialPaymentText { get; set; }
        public int? InstallmentPayment { get; set; }
        public string? InstallmentPaymentText { get; set; }
        public string? ProcessDescription { get; set; }
        public bool? IsCLevel { get; set; }
    }

    public class DemandOfferViewModel
    {
        public long DemandOfferId { get; set; }
        public long DemandId { get; set; }
        public long CurrencyTypeId { get; set; }
        public long RequestInfoId { get; set; }
        public decimal? TotalPrice { get; set; }
        public int? Status { get; set; }
        public string? CompanyName { get; set; }
        public string? CompanyPhone { get; set; }
        public string? CompanyAddress { get; set; }
        public string? Quantity { get; set; }
        public decimal? Price { get; set; }
        public string SupplierName { get; set; }
        public string SupplierTaxNumber { get; set; }
        public string SupplierPhone { get; set; }
        public long SupplierId { get; set; }
        public string? SupplierAdress { get; set; }
        public decimal?  ExchangeRate { get; set; }
        public int UnitManager { get; set; }
        public DateTime?  DeadlineDate{ get; set; }
        public DateTime? MaturityDate { get; set; }
        public int? PaymentType { get; set; }
        public int? InstallmentPayment { get; set; }
        public int? PartialPayment { get; set; }
        public List<IFormFile>? Files { get; set; }
        public string? File1Name { get; set; }
        public List<OfferRequestViewModel>? RequestInfoViewModels { get; set; }
    }

    public class RequestInfoViewModel
    {
        public string Metarial { get; set; }
        public int? Quantity { get; set; }
        public string Unit { get; set; }
    }
    public class Product
    {
        public string? Name { get; set; }
        public string? ProductCode { get; set; }
        public List<Product>? SubProducts { get; set; }
        public string? Description { get; set; }
    }
}
