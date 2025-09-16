using Demand.Business.Abstract.InvoiceService;
using Demand.Business.Abstract.PersonnelService;
using Demand.Core.Attribute;
using Demand.Core.DatabaseConnection.NebimConnection;
using Demand.Domain.Entities.InvoiceEntity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Demand.Presentation.Controllers
{
    [UserToken]
    public class InvoiceController : Controller
    {
        private readonly ILogger<InvoiceController> _logger;
        private readonly IInvoiceDetailService _invoiceDetailService;
        private readonly IPersonnelService _personnelService;

        public InvoiceController(ILogger<InvoiceController> logger, IInvoiceDetailService invoiceDetailService, IPersonnelService personnelService)
        {
            _logger = logger;
            _invoiceDetailService = invoiceDetailService;
            _personnelService = personnelService;
        }

        private long GetUserId()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            return long.Parse(claimsIdentity.Claims.First(x => x.Type == "UserId").Value);
        }

        public IActionResult Index()
        {
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
                    matchingDetail.ResponsiblePerson = personnelList.FirstOrDefault(x=>x.Id == matchingDetail.ResponsiblePersonId);
                    matchingDetail.SentToUser = matchingDetail.SentToUserId != null ? personnelList.FirstOrDefault(x => x.Id == matchingDetail.SentToUserId.Value) : null;
                    headerModel.Status = matchingDetail.Status;

                    headerModel.InvoiceDetailEntity = matchingDetail;
                }
            }
            return View(IncomingEInvoiceHeaderModels);
        }

        [HttpGet]
        public IActionResult GetInvoiceDetail(Guid invoiceHeaderId)
        {
            var nebimConnection = new NebimConnection();
            var IncomingEInvoiceLineModels = nebimConnection.GetIncomingEInvoiceLineModels(invoiceHeaderId);

            return Json(IncomingEInvoiceLineModels);
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

            invoice.SentToUserId = dto.ApprovedPersonId;
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
        public long ApprovedPersonId { get; set; }
    }

    // DTO
    public class InvoiceRejectionDto
    {
        public Guid Id { get; set; }
        public required string RejectionDesc { get; set; }
    }
}
