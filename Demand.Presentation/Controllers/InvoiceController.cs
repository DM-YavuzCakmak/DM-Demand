using Demand.Business.Abstract.Department;
using Demand.Business.Abstract.InvoiceService;
using Demand.Business.Abstract.PersonnelService;
using Demand.Business.Abstract.ProductCategoryService;
using Demand.Business.Abstract.Provider;
using Demand.Business.Concrete.DepartmentService;
using Demand.Core.Attribute;
using Demand.Core.DatabaseConnection.NebimConnection;
using Demand.Domain.DTO;
using Demand.Domain.Entities.InvoiceEntity;
using Demand.Domain.NebimModels;
using Demand.Domain.ViewModels;
using Demand.Infrastructure.DataAccess.Concrete.EntityFramework.Contexts;
using DocumentFormat.OpenXml.Bibliography;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Data;
using System.Security.Claims;

namespace Demand.Presentation.Controllers
{
    [UserToken]
    public class InvoiceController : Controller
    {
        private readonly DemandContext _dbContext;
        private readonly ILogger<InvoiceController> _logger;
        private readonly IInvoiceDetailService _invoiceDetailService;
        private readonly IInvoiceDemandService _invoiceDemandService;
        private readonly IDepartmentService _departmentService;
        private readonly IPersonnelService _personnelService;
        private readonly IProductCategoryService _productCategoryService;

        public InvoiceController(ILogger<InvoiceController> logger, IInvoiceDetailService invoiceDetailService, IInvoiceDemandService invoiceDemandService, IPersonnelService personnelService, IProductCategoryService productCategoryService, IDepartmentService departmentService, DemandContext dbContext)
        {
            _logger = logger;
            _invoiceDetailService = invoiceDetailService;
            _invoiceDemandService = invoiceDemandService;
            _personnelService = personnelService;
            _productCategoryService = productCategoryService;
            _departmentService = departmentService;
            _dbContext = dbContext;
        }

        private long GetUserId()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            return long.Parse(claimsIdentity.Claims.First(x => x.Type == "UserId").Value);
        }

        public IActionResult New()
        {
            ViewData["ActivePage"] = "InvoiceNew";
            var nebimConnection = new NebimConnection();
            var IncomingEInvoiceHeaderModels = nebimConnection.GetIncomingEInvoiceHeaderModels();

            var personnelList = _personnelService.GetList()?.Data;
            ViewBag.PersonnelList = personnelList;

            var invoiceDetails = _invoiceDetailService.GetAll()?.Data;
            foreach (var headerModel in IncomingEInvoiceHeaderModels)
            {
                var matchingDetail = invoiceDetails?.FirstOrDefault(d => d.InvoiceUUID == headerModel.InvoiceHeaderID);
                if (matchingDetail != null)
                {
                    matchingDetail.ResponsiblePerson = personnelList.FirstOrDefault(x => x.Id == matchingDetail.ResponsiblePersonId);
                    matchingDetail.SentToUser = matchingDetail.SentToUserId != null ? personnelList.FirstOrDefault(x => x.Id == matchingDetail.SentToUserId.Value) : null;
                    headerModel.Status = matchingDetail.Status;

                    headerModel.InvoiceDetailEntity = matchingDetail;
                }
            }
            return View(IncomingEInvoiceHeaderModels);
        }

        public IActionResult Control()
        {
            ViewData["ActivePage"] = "InvoiceControl";
            var nebimConnection = new NebimConnection();
            var IncomingEInvoiceHeaderModels = nebimConnection.GetIncomingEInvoiceHeaderModels();

            var personnelList = _personnelService.GetList()?.Data;

            var departmentEntities = _departmentService.GetAll()?.Data;
            ViewBag.DepartmentList = departmentEntities;

            var invoiceDetails = _invoiceDetailService.GetAll()?.Data;
            foreach (var headerModel in IncomingEInvoiceHeaderModels)
            {
                var matchingDetail = invoiceDetails?.FirstOrDefault(d => d.InvoiceUUID == headerModel.InvoiceHeaderID);
                if (matchingDetail != null)
                {
                    matchingDetail.ResponsiblePerson = personnelList.FirstOrDefault(x => x.Id == matchingDetail.ResponsiblePersonId);
                    matchingDetail.SentToUser = matchingDetail.SentToUserId != null ? personnelList.FirstOrDefault(x => x.Id == matchingDetail.SentToUserId.Value) : null;
                    headerModel.Status = matchingDetail.Status;

                    headerModel.InvoiceDetailEntity = matchingDetail;
                }
            }
            return View(IncomingEInvoiceHeaderModels);
        }

        public IActionResult Approved()
        {
            ViewData["ActivePage"] = "InvoiceApproved";
            var nebimConnection = new NebimConnection();
            var IncomingEInvoiceHeaderModels = nebimConnection.GetIncomingEInvoiceHeaderModels();

            var personnelList = _personnelService.GetList()?.Data;
            ViewBag.PersonnelList = personnelList;

            var invoiceDetails = _invoiceDetailService.GetAll()?.Data;
            foreach (var headerModel in IncomingEInvoiceHeaderModels)
            {
                var matchingDetail = invoiceDetails?.FirstOrDefault(d => d.InvoiceUUID == headerModel.InvoiceHeaderID);
                if (matchingDetail != null)
                {
                    matchingDetail.ResponsiblePerson = personnelList.FirstOrDefault(x => x.Id == matchingDetail.ResponsiblePersonId);
                    matchingDetail.SentToUser = matchingDetail.SentToUserId != null ? personnelList.FirstOrDefault(x => x.Id == matchingDetail.SentToUserId.Value) : null;
                    headerModel.Status = matchingDetail.Status;

                    headerModel.InvoiceDetailEntity = matchingDetail;
                }
            }

            if (!IncomingEInvoiceHeaderModels.Any())
                IncomingEInvoiceHeaderModels.Add(new IncomingEInvoiceHeaderModel());

            IncomingEInvoiceHeaderModels[0].NebimCategoryModels = nebimConnection.GetNebimCategoryModels();
            IncomingEInvoiceHeaderModels[0].NebimSubCategoryModels = nebimConnection.GetNebimSubCategoryModels();
            IncomingEInvoiceHeaderModels[0].NebimProductModels = nebimConnection.GetNebimProductModels();

            ViewBag.ProductCategories = _productCategoryService.GetAll().Data.ToList();

            return View(IncomingEInvoiceHeaderModels);
        }

        public IActionResult Pending()
        {
            ViewData["ActivePage"] = "InvoicePending";
            var nebimConnection = new NebimConnection();
            var IncomingEInvoiceHeaderModels = nebimConnection.GetIncomingEInvoiceHeaderModels();

            var personnelList = _personnelService.GetList()?.Data;
            ViewBag.PersonnelList = personnelList;

            var invoiceDetails = _invoiceDetailService.GetAll()?.Data;
            foreach (var headerModel in IncomingEInvoiceHeaderModels)
            {
                var matchingDetail = invoiceDetails?.FirstOrDefault(d => d.InvoiceUUID == headerModel.InvoiceHeaderID);
                if (matchingDetail != null)
                {
                    matchingDetail.ResponsiblePerson = personnelList.FirstOrDefault(x => x.Id == matchingDetail.ResponsiblePersonId);
                    matchingDetail.SentToUser = matchingDetail.SentToUserId != null ? personnelList.FirstOrDefault(x => x.Id == matchingDetail.SentToUserId.Value) : null;
                    headerModel.Status = matchingDetail.Status;

                    headerModel.InvoiceDetailEntity = matchingDetail;
                }
            }
            return View(IncomingEInvoiceHeaderModels);
        }

        public IActionResult Reject()
        {
            ViewData["ActivePage"] = "InvoiceReject";
            var nebimConnection = new NebimConnection();
            var IncomingEInvoiceHeaderModels = nebimConnection.GetIncomingEInvoiceHeaderModels();

            var personnelList = _personnelService.GetList()?.Data;
            ViewBag.PersonnelList = personnelList;

            var invoiceDetails = _invoiceDetailService.GetAll()?.Data;
            foreach (var headerModel in IncomingEInvoiceHeaderModels)
            {
                var matchingDetail = invoiceDetails?.FirstOrDefault(d => d.InvoiceUUID == headerModel.InvoiceHeaderID);
                if (matchingDetail != null)
                {
                    matchingDetail.ResponsiblePerson = personnelList.FirstOrDefault(x => x.Id == matchingDetail.ResponsiblePersonId);
                    matchingDetail.SentToUser = matchingDetail.SentToUserId != null ? personnelList.FirstOrDefault(x => x.Id == matchingDetail.SentToUserId.Value) : null;
                    headerModel.Status = matchingDetail.Status;

                    headerModel.InvoiceDetailEntity = matchingDetail;
                }
            }
            return View(IncomingEInvoiceHeaderModels);
        }

        public IActionResult NebimSend(Guid invoiceHeaderId)
        {
            ViewData["ActivePage"] = "NebimSend";

            var nebimConnection = new NebimConnection();
            var incomingEInvoiceHeaderModel = nebimConnection.GetIncomingEInvoiceHeaderModels(invoiceHeaderId).FirstOrDefault();
            if (incomingEInvoiceHeaderModel == null)
                return NotFound(new { success = false, message = "Invoice not found" });

            var companyId = incomingEInvoiceHeaderModel.CompanyName == "DEM" ? 1 : 2;
            ViewBag.NebimOfficeModels = nebimConnection.GetOfficeList(companyId);
            ViewBag.NebimWareHouseModels = nebimConnection.GetWareHouseList(companyId);
            ViewBag.NebimExpenseModels = nebimConnection.GetExpenseList(companyId);
            ViewBag.NebimCostModels = nebimConnection.GetCostList(companyId);

            return View(incomingEInvoiceHeaderModel);
        }

        public IActionResult NebimProductSend(Guid invoiceHeaderId)
        {
            ViewData["ActivePage"] = "NebimProductSend";

            var nebimConnection = new NebimConnection();
            var incomingEInvoiceHeaderModel = nebimConnection.GetIncomingEInvoiceHeaderModels(invoiceHeaderId).FirstOrDefault();
            if (incomingEInvoiceHeaderModel == null)
                return NotFound(new { success = false, message = "Invoice not found" });

            var companyId = incomingEInvoiceHeaderModel.CompanyName == "DEM" ? 1 : 2;
            ViewBag.NebimOfficeModels = nebimConnection.GetOfficeList(companyId);
            ViewBag.NebimWareHouseModels = nebimConnection.GetWareHouseList(companyId);

            ViewBag.NebimCategoryModels = nebimConnection.GetNebimCategoryModels();
            ViewBag.NebimSubCategoryModels = nebimConnection.GetNebimSubCategoryModels();
            ViewBag.NebimProductModels = nebimConnection.GetNebimProductModels().Where(x => x.CompanyName == incomingEInvoiceHeaderModel.CompanyName);

            ViewBag.ProductCategories = _productCategoryService.GetAll().Data.ToList();
            return View(incomingEInvoiceHeaderModel);
        }

        [HttpGet]
        public IActionResult GetInvoiceDetail(Guid invoiceHeaderId)
        {
            var nebimConnection = new NebimConnection();
            var IncomingEInvoiceLineModels = nebimConnection.GetIncomingEInvoiceLineModels(invoiceHeaderId);

            return Json(IncomingEInvoiceLineModels);
        }

        [HttpGet]
        public IActionResult GetInvoiceDetails(Guid invoiceHeaderId)
        {
            var nebimConnection = new NebimConnection();
            var incomingEInvoiceHeaderModel = nebimConnection.GetIncomingEInvoiceHeaderModels(invoiceHeaderId).FirstOrDefault();
            if (incomingEInvoiceHeaderModel == null)
                return NotFound(new { success = false, message = "Invoice not found" });

            var incomingEInvoiceLineModels = nebimConnection.GetIncomingEInvoiceLineModels(invoiceHeaderId);
            GetInvoiceDemandResponse getInvoiceDemandResponse = new GetInvoiceDemandResponse
            {
                Invoice = incomingEInvoiceHeaderModel,
                InvoiceDetails = incomingEInvoiceLineModels
            };

            return Json(getInvoiceDemandResponse);
        }

        [HttpGet]
        public IActionResult GetInvoiceDemands(Guid invoiceHeaderId, bool isApproved = false)
        {
            var nebimConnection = new NebimConnection();
            var incomingEInvoiceHeaderModel = nebimConnection.GetIncomingEInvoiceHeaderModels(invoiceHeaderId).FirstOrDefault();
            if (incomingEInvoiceHeaderModel == null)
                return NotFound(new { success = false, message = "Invoice not found" });

            var invoice = _invoiceDetailService.GetByUUID(invoiceHeaderId)?.Data;
            if (invoice == null)
                return NotFound(new { success = false, message = "Invoice not found" });

            var incomingEInvoiceLineModels = nebimConnection.GetIncomingEInvoiceLineModels(invoiceHeaderId);
            var demands = _dbContext.GetDemandsByTaxNumber(incomingEInvoiceHeaderModel.TaxNumber.Trim() ?? incomingEInvoiceHeaderModel.IdentityNum.Trim(), invoice.Id);

            GetInvoiceDemandResponse getInvoiceDemandResponse = new GetInvoiceDemandResponse
            {
                Invoice = incomingEInvoiceHeaderModel,
                Demands = isApproved ? demands.Where(x => x.MatchingPrice != null && x.MatchingPrice > 0).ToList() : demands.Where(x => x.RemainingPrice > 0).ToList(),
                InvoiceDetails = incomingEInvoiceLineModels
            };

            return Json(getInvoiceDemandResponse);
        }

        public class GetInvoiceDemandResponse
        {
            public required IncomingEInvoiceHeaderModel Invoice { get; set; }
            public List<DemandsByTaxNumberDto>? Demands { get; set; }
            public List<IncomingEInvoiceLineModel> InvoiceDetails { get; set; }
        }

        [HttpPut]
        public async Task<IActionResult> SaveInvoiceDemand([FromBody] List<InvoiceDemandDto> dto)
        {
            var invoice = _invoiceDetailService.GetByUUID(dto[0].InvoiceId)?.Data;
            if (invoice == null)
                return NotFound(new { success = false, message = "Invoice not found" });

            var deleteResult = _invoiceDemandService.DeleteByInvoiceId(invoice.Id);
            if (!deleteResult)
                return BadRequest(new { success = false, message = "Failed to delete existing invoice demands" });

            foreach (var item in dto)
            {
                InvoiceDemandEntity invoiceDemand = new InvoiceDemandEntity
                {
                    InvoiceId = invoice.Id,
                    DemandId = item.DemandId,
                    TotalPrice = item.Amount,
                    IsDeleted = false,
                    CreatedAt = GetUserId(),
                    CreatedDate = DateTime.Now
                };
                _invoiceDemandService.Add(invoiceDemand);
            }

            return Ok(new { success = true, message = "Invoice demands saved successfully" });
        }

        [HttpPut]
        public async Task<IActionResult> InvoiceUpdate([FromBody] InvoiceDetailUpdateDto dto)
        {
            var invoiceOld = _invoiceDetailService.GetByUUID(dto.Id)?.Data;
            if (invoiceOld != null)
                return NotFound(new { success = false, message = "Invoice not found" });

            InvoiceDetailEntity invoice = new InvoiceDetailEntity();

            invoice.InvoiceUUID = dto.Id;
            invoice.ResponsiblePersonId = dto.ResponsiblePersonId;
            invoice.InvoiceType = dto.InvoiceType;
            invoice.Status = 0;
            invoice.IsDeleted = false;
            invoice.CreatedAt = GetUserId();
            invoice.CreatedDate = DateTime.Now;

            _invoiceDetailService.Add(invoice);

            return Ok(new { success = true, message = "Invoice updated successfully" });
        }

        [HttpPut]
        public async Task<IActionResult> InvoiceApprovedUpdate([FromBody] InvoiceApprovedUpdateDto dto)
        {
            var invoice = _invoiceDetailService.GetByUUID(dto.Id)?.Data;
            if (invoice == null)
                return NotFound(new { success = false, message = "Invoice not found" });

            invoice.SentToDeparmentId = dto.ApprovedDepartmentId;
            invoice.Status = 0;
            invoice.UpdatedAt = GetUserId();
            invoice.UpdatedDate = DateTime.Now;

            _invoiceDetailService.Update(invoice);

            return Ok(new { success = true, message = "Invoice updated approved user successfully" });
        }

        [HttpPut]
        public async Task<IActionResult> InvoiceRejection([FromBody] InvoiceRejectionDto dto)
        {
            var invoice = _invoiceDetailService.GetByUUID(dto.Id)?.Data;
            if (invoice == null)
                return NotFound(new { success = false, message = "Invoice not found" });

            invoice.Status = 1;
            invoice.IsDeleted = false;
            invoice.ReturnDate = DateTime.Now;
            invoice.RejectionNote = dto.RejectionDesc;
            invoice.UpdatedAt = GetUserId();
            invoice.UpdatedDate = DateTime.Now;

            _invoiceDetailService.Update(invoice);

            return Ok(new { success = true, message = "Invoice rejection successfully" });
        }

        [HttpGet]
        public IActionResult InvoiceApproved(Guid invoiceHeaderId)
        {
            var invoice = _invoiceDetailService.GetByUUID(invoiceHeaderId)?.Data;
            if (invoice == null)
                return NotFound(new { success = false, message = "Invoice not found" });

            invoice.Status = 2;
            invoice.IsDeleted = false;
            invoice.ReturnDate = DateTime.Now;
            invoice.UpdatedAt = GetUserId();
            invoice.UpdatedDate = DateTime.Now;

            _invoiceDetailService.Update(invoice);

            return Ok(new { success = true, message = "Invoice approved successfully" });
        }
    }

    // DTO
    public class InvoiceDemandDto
    {
        public Guid InvoiceId { get; set; }
        public long DemandId { get; set; }
        public decimal Amount { get; set; }
    }

    // DTO
    public class InvoiceDetailUpdateDto
    {
        public Guid Id { get; set; }
        public int InvoiceType { get; set; }
        public long ResponsiblePersonId { get; set; }
    }

    // DTO
    public class InvoiceApprovedUpdateDto
    {
        public Guid Id { get; set; }
        public long ApprovedDepartmentId { get; set; }
    }

    // DTO
    public class InvoiceRejectionDto
    {
        public Guid Id { get; set; }
        public required string RejectionDesc { get; set; }
    }
}
