using Demand.Business.Abstract.Department;
using Demand.Business.Abstract.InvoiceService;
using Demand.Business.Abstract.PersonnelDepartmentService;
using Demand.Business.Abstract.PersonnelService;
using Demand.Business.Abstract.ProductCategoryService;
using Demand.Business.Concrete.PersonnelDepartmentService;
using Demand.Core.Attribute;
using Demand.Core.DatabaseConnection.NebimConnection;
using Demand.Core.Utilities.Email;
using Demand.Domain.DTO;
using Demand.Domain.Entities.Demand;
using Demand.Domain.Entities.DemandProcess;
using Demand.Domain.Entities.InvoiceEntity;
using Demand.Domain.Entities.Personnel;
using Demand.Domain.Enums;
using Demand.Domain.NebimModels;
using Demand.Infrastructure.DataAccess.Concrete.EntityFramework.Contexts;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IInvoiceProcessService _invoiceProcessService;
        private readonly IInvoiceDemandService _invoiceDemandService;
        private readonly IDepartmentService _departmentService;
        private readonly IPersonnelService _personnelService;
        private readonly IPersonnelDepartmentService _personnelDepartmentService;
        private readonly IProductCategoryService _productCategoryService;

        public InvoiceController(ILogger<InvoiceController> logger, IInvoiceDetailService invoiceDetailService, IInvoiceProcessService invoiceProcessService, IInvoiceDemandService invoiceDemandService, IPersonnelService personnelService, IProductCategoryService productCategoryService, IDepartmentService departmentService, IPersonnelDepartmentService personnelDepartmentService, DemandContext dbContext)
        {
            _logger = logger;
            _invoiceDetailService = invoiceDetailService;
            _invoiceProcessService = invoiceProcessService;
            _invoiceDemandService = invoiceDemandService;
            _personnelService = personnelService;
            _productCategoryService = productCategoryService;
            _departmentService = departmentService;
            _personnelDepartmentService = personnelDepartmentService;
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

            return View(IncomingEInvoiceHeaderModels);
        }

        public IActionResult Control()
        {
            ViewData["ActivePage"] = "InvoiceControl";
            var nebimConnection = new NebimConnection();
            var IncomingEInvoiceHeaderModels = nebimConnection.GetIncomingEInvoiceHeaderModels();

            var departmentEntities = _departmentService.GetAll()?.Data;
            ViewBag.DepartmentList = departmentEntities;

            var invoiceDetails = _invoiceDetailService.GetAll()?.Data;
            foreach (var headerModel in IncomingEInvoiceHeaderModels)
            {
                var matchingDetail = invoiceDetails?.FirstOrDefault(d => d.InvoiceUUID == headerModel.InvoiceHeaderID);
                if (matchingDetail != null)
                {
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

            var departmentEntities = _departmentService.GetAll()?.Data;
            ViewBag.DepartmentList = departmentEntities;

            var thisPersonnel = _personnelService.GetById(GetUserId())?.Data;

            var thisPersonelDepartmentList = _personnelDepartmentService.GetList(x => x.PersonnelId == GetUserId() && x.IsDeleted == false)?.Data;

            var invoiceDetails = _invoiceDetailService.GetAll()?.Data;
            foreach (var headerModel in IncomingEInvoiceHeaderModels)
            {
                var matchingDetail = invoiceDetails?.FirstOrDefault(d => d.InvoiceUUID == headerModel.InvoiceHeaderID);
                if (matchingDetail != null)
                {
                    headerModel.IsViewInvoicePrivilege =
                        thisPersonelDepartmentList != null
                        &&
                        thisPersonnel != null
                        &&
                        thisPersonelDepartmentList.Any(x => x.DepartmentId == matchingDetail.SentToDepartmentId || (thisPersonnel.ParentId != null && x.DepartmentId == 18) || (matchingDetail.InvoiceType != 2 && thisPersonnel.Id == 50))// Departmandaki herkes görsün, tipi uygun ise Emre ve YK hariç tüm mali işlere bağlı personel görsün
                        &&
                        matchingDetail.Status == (int)InvoiceStatusEnum.Approved;


                    headerModel.IsViewInvoicePrivilege =
                        thisPersonelDepartmentList != null
                        &&
                        thisPersonnel != null
                        &&
                        thisPersonelDepartmentList.Any(x =>
                            (thisPersonnel.ParentId != null && x.DepartmentId == matchingDetail.SentToDepartmentId) ||
                            (thisPersonnel.ParentId != null && x.DepartmentId == 18) || 
                            (matchingDetail.InvoiceType != 2 && thisPersonnel.Id == 50))
                        // Departmandaki herkes görsün ve YK hariç tüm mali işlere bağlı personel görsün
                        &&
                        matchingDetail.Status == (int)InvoiceStatusEnum.Approved;

                    if (headerModel.IsViewInvoicePrivilege)
                    {
                        matchingDetail.SentToDepartment = matchingDetail.SentToDepartmentId != null ? departmentEntities.FirstOrDefault(x => x.Id == matchingDetail.SentToDepartmentId.Value) : null;
                        headerModel.Status = matchingDetail.Status;

                        headerModel.IsChangeStatusPrivilege =
                            thisPersonelDepartmentList != null
                            &&
                            thisPersonnel != null
                            &&
                            (
                                (matchingDetail.InvoiceType != 2 && thisPersonnel.Id == 50) ||
                                (matchingDetail.InvoiceType == 2 && thisPersonelDepartmentList.Any(x => thisPersonnel.ParentId != null && x.DepartmentId == 18))
                            );
                    }
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

            var departmentEntities = _departmentService.GetAll()?.Data;
            ViewBag.DepartmentList = departmentEntities;

            var thisPersonnel = _personnelService.GetById(GetUserId())?.Data;

            var thisPersonelDepartmentList = _personnelDepartmentService.GetList(x => x.PersonnelId == GetUserId() && x.IsDeleted == false)?.Data;

            var invoiceDetails = _invoiceDetailService.GetAll()?.Data;
            foreach (var headerModel in IncomingEInvoiceHeaderModels)
            {
                var matchingDetail = invoiceDetails?.FirstOrDefault(d => d.InvoiceUUID == headerModel.InvoiceHeaderID);
                if (matchingDetail != null)
                {
                    headerModel.IsViewInvoicePrivilege =
                        thisPersonelDepartmentList != null
                        &&
                        thisPersonnel != null
                        &&
                        thisPersonelDepartmentList.Any(x => 
                            (thisPersonnel.ParentId != null && x.DepartmentId == matchingDetail.SentToDepartmentId) || 
                            (thisPersonnel.ParentId != null && x.DepartmentId == 18) || 
                            (matchingDetail.Status == (int)InvoiceStatusEnum.SecondLevelApproved && thisPersonelDepartmentList.Any(x => x.DepartmentId ==   matchingDetail.SentToDepartmentId && x.ApproveLevel == 3)))
                            // Departmandaki herkes görsün ve YK hariç tüm mali işlere bağlı personel görsün
                        &&
                        (matchingDetail.Status == (int)InvoiceStatusEnum.Pending ||
                        matchingDetail.Status == (int)InvoiceStatusEnum.FirstLevelApproved ||
                        matchingDetail.Status == (int)InvoiceStatusEnum.SecondLevelApproved);

                    if (headerModel.IsViewInvoicePrivilege)
                    {
                        matchingDetail.SentToDepartment = matchingDetail.SentToDepartmentId != null ? departmentEntities.FirstOrDefault(x => x.Id == matchingDetail.SentToDepartmentId.Value) : null;
                        headerModel.Status = matchingDetail.Status;

                        headerModel.IsChangeStatusPrivilege =
                            thisPersonelDepartmentList != null
                            &&
                            (
                                (matchingDetail.Status == (int)InvoiceStatusEnum.Pending
                                &&
                                thisPersonelDepartmentList.Any(x => x.DepartmentId == matchingDetail.SentToDepartmentId && x.ApproveLevel == 1))
                            ||
                                (matchingDetail.Status == (int)InvoiceStatusEnum.FirstLevelApproved
                                &&
                                thisPersonelDepartmentList.Any(x => x.DepartmentId == matchingDetail.SentToDepartmentId && x.ApproveLevel == 2))
                            ||
                                (matchingDetail.Status == (int)InvoiceStatusEnum.SecondLevelApproved
                                &&
                                thisPersonelDepartmentList.Any(x => x.DepartmentId == matchingDetail.SentToDepartmentId && x.ApproveLevel == 3))
                            );
                    }

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

            var departmentEntities = _departmentService.GetAll()?.Data;
            ViewBag.DepartmentList = departmentEntities;

            var invoiceDetails = _invoiceDetailService.GetAll()?.Data;
            foreach (var headerModel in IncomingEInvoiceHeaderModels)
            {
                var matchingDetail = invoiceDetails?.FirstOrDefault(d => d.InvoiceUUID == headerModel.InvoiceHeaderID);
                if (matchingDetail != null)
                {
                    matchingDetail.SentToDepartment = matchingDetail.SentToDepartmentId != null ? departmentEntities.FirstOrDefault(x => x.Id == matchingDetail.SentToDepartmentId.Value) : null;
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

            InvoiceProcessEntity invoiceProcess = new InvoiceProcessEntity
            {
                InvoiceDetailId = invoice.Id,
                ProcessType = (int)InvoiceProcessTypeEnum.MatchingToDemand,
                CreatedAt = GetUserId(),
                CreatedDate = DateTime.Now
            };

            _invoiceProcessService.Add(invoiceProcess);

            return Ok(new { success = true, message = "Invoice demands saved successfully" });
        }

        [HttpPut]
        public async Task<IActionResult> InvoiceUpdate([FromBody] InvoiceDetailUpdateDto dto)
        {
            var invoiceOld = _invoiceDetailService.GetByUUID(dto.Id)?.Data;
            if (invoiceOld != null)
                return NotFound(new { success = false, message = "Invoice not found" });

            var eInvoiceHeaderModel = new NebimConnection().GetIncomingEInvoiceHeaderModels(dto.Id).FirstOrDefault();

            InvoiceDetailEntity invoice = new InvoiceDetailEntity();

            invoice.InvoiceUUID = dto.Id;
            invoice.InvoiceType = dto.InvoiceType;
            invoice.Status = (int)InvoiceStatusEnum.New;
            invoice.IsDeleted = false;
            invoice.CreatedAt = GetUserId();
            invoice.CreatedDate = DateTime.Now;

            _invoiceDetailService.Add(invoice);

            if (dto.InvoiceType == 0 || dto.InvoiceType == 1)
            {
                var responsibleUser = _personnelService.GetById(50)?.Data;
                var thisUser = _personnelService.GetById(GetUserId())?.Data;

                string demandLink = "https://portal.demmuseums.com/Invoice/Control";
                var emailBody = $"Merhabalar Sayın {responsibleUser.FirstName} {responsibleUser.LastName},<br/><br/>" +
                                $"{thisUser.FirstName} {thisUser.LastName} tarafından, {eInvoiceHeaderModel.EInvoiceNumber} numaralı faturanın tipi belirlenmiş ve incelenmek üzere tarafınıza iletilmiştir. Aşağıdaki linkten faturayı kontrol ederek ilgili departmana yönlendirmenizi rica ederiz.<br/><br/>" +
                                $"<a href='{demandLink}'> Fatura Görüntüle </a> <br/><br/>" +
                                "Saygılarımızla.";
                EmailHelper.SendEmail(new List<string> { responsibleUser.Email }, "Kontrol Bekleyen Fatura", emailBody);
            }

            InvoiceProcessEntity invoiceProcess = new InvoiceProcessEntity
            {
                InvoiceDetailId = invoice.Id,
                ProcessType = (int)InvoiceProcessTypeEnum.InvoiceTypeChange,
                CreatedAt = GetUserId(),
                CreatedDate = DateTime.Now
            };

            _invoiceProcessService.Add(invoiceProcess);

            return Ok(new { success = true, message = "Invoice type updated successfully" });
        }

        [HttpPut]
        public async Task<IActionResult> InvoiceReturn([FromBody] InvoiceRejectionDto dto)
        {
            var invoice = _invoiceDetailService.GetByUUID(dto.Id)?.Data;
            if (invoice == null)
                return NotFound(new { success = false, message = "Invoice not found" });

            invoice.IsDeleted = true;
            invoice.ReturnDate = DateTime.Now;
            invoice.UpdatedAt = GetUserId();
            invoice.UpdatedDate = DateTime.Now;

            _invoiceDetailService.Update(invoice);

            InvoiceProcessEntity invoiceProcess = new InvoiceProcessEntity
            {
                InvoiceDetailId = invoice.Id,
                ProcessType = (int)InvoiceProcessTypeEnum.Return,
                CreatedAt = GetUserId(),
                CreatedDate = DateTime.Now
            };

            _invoiceProcessService.Add(invoiceProcess);

            return Ok(new { success = true, message = "Invoice return successfully" });
        }

        public class DepartmentUsersResponseDto
        {
            public List<SelectItemDto> Users { get; set; } = new();
            public List<SelectItemDto> Managers { get; set; } = new();
        }

        public class SelectItemDto
        {
            public long Id { get; set; }
            public string Text { get; set; } = "";
            public bool IsFirstCheck { get; set; }
        }

        [HttpPut]
        public async Task<IActionResult> InvoiceApprovedUpdate([FromBody] InvoiceApprovedUpdateDto dto)
        {
            var invoice = _invoiceDetailService.GetByUUID(dto.Id)?.Data;
            if (invoice == null)
                return NotFound(new { success = false, message = "Invoice not found" });

            var eInvoiceHeaderModel = new NebimConnection().GetIncomingEInvoiceHeaderModels(dto.Id).FirstOrDefault();

            invoice.SentToDepartmentId = dto.ApprovedDepartmentId;
            invoice.Status = (int)InvoiceStatusEnum.Pending;
            invoice.UpdatedAt = GetUserId();
            invoice.UpdatedDate = DateTime.Now;

            _invoiceDetailService.Update(invoice);

            var personnelDepartmentList = _personnelDepartmentService.GetList(x => x.IsDeleted == false && x.DepartmentId == dto.ApprovedDepartmentId && x.ApproveLevel == 1).Data;
            if (personnelDepartmentList.Any())
            {
                var department = _departmentService.GetById(dto.ApprovedDepartmentId)?.Data;
                var responsibleUser = _personnelService.GetById(GetUserId())?.Data;
                var departmentUsers = _personnelService.GetList(x => personnelDepartmentList.Select(p => p.PersonnelId).Contains(x.Id))?.Data;

                string demandLink = "https://portal.demmuseums.com/Invoice/Pending";
                var emailBody = $"Merhabalar,<br/><br/>" +
                                $"{responsibleUser.FirstName} {responsibleUser.LastName} tarafından, {eInvoiceHeaderModel.EInvoiceNumber} numaralı fatura {department.Name} departmanınıza yönlendirilmiştir. Departmanınıza ait ilk seviye onay verecek kişiler listesinde bulunmanız sebebiyle bu mail tarafınıza iletilmiştir. Aşağıdaki linkten faturayı kontrol ederek onay/ret vermenizi rica ederiz.<br/><br/>" +
                                $"<a href='{demandLink}'> Fatura Görüntüle </a> <br/><br/>" +
                                "Saygılarımızla.";
                EmailHelper.SendEmail(departmentUsers.Select(x => x.Email).ToList(), "Onay Bekleyen Fatura", emailBody);
            }

            InvoiceProcessEntity invoiceProcess = new InvoiceProcessEntity
            {
                InvoiceDetailId = invoice.Id,
                ProcessType = (int)InvoiceProcessTypeEnum.SentToDepartment,
                CreatedAt = GetUserId(),
                CreatedDate = DateTime.Now
            };

            _invoiceProcessService.Add(invoiceProcess);

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

            InvoiceProcessEntity invoiceProcess = new InvoiceProcessEntity
            {
                InvoiceDetailId = invoice.Id,
                ProcessType = (int)InvoiceProcessTypeEnum.Reject,
                CreatedAt = GetUserId(),
                CreatedDate = DateTime.Now
            };

            _invoiceProcessService.Add(invoiceProcess);

            return Ok(new { success = true, message = "Invoice rejection successfully" });
        }

        [HttpGet]
        public IActionResult InvoiceApproved(Guid invoiceHeaderId)
        {
            var invoice = _invoiceDetailService.GetByUUID(invoiceHeaderId)?.Data;
            if (invoice == null)
                return NotFound(new { success = false, message = "Invoice not found" });

            //invoice.Status = invoice.Status == 0 && invoice.SentToUserId != null && invoice.SentToManagerId != null ? 3 : 2;
            invoice.IsDeleted = false;
            invoice.ReturnDate = DateTime.Now;
            invoice.UpdatedAt = GetUserId();
            invoice.UpdatedDate = DateTime.Now;

            _invoiceDetailService.Update(invoice);

            InvoiceProcessEntity invoiceProcess = new InvoiceProcessEntity
            {
                InvoiceDetailId = invoice.Id,
                ProcessType = (int)InvoiceProcessTypeEnum.FirstLevelApprove,
                CreatedAt = GetUserId(),
                CreatedDate = DateTime.Now
            };

            _invoiceProcessService.Add(invoiceProcess);

            return Ok(new { success = true, message = "Invoice approved successfully" });
        }

        //public ResponseConnect GetSession()
        //{
        //    var path = _configuration.SessionUrl + _configuration.Connect;

        //    var response = HttpProvider.HttpGetWithResponse<ResponseConnect>(path);

        //    if (response != null && response.StatusCode == 200)
        //    {
        //        NebimConfiguration.SessionId = response.SessionId;
        //    }

        //    return response;
        //}

        private RequestCreateWholesaleInvoice SalesInvoiceRequest()
        {
            var invoiceRequest = new RequestCreateWholesaleInvoice
            {
                ModelType = 19,
                CompanyCode = "1",
                InvoiceDate = "04-02-2026", //invoice.InvoiceDate.ToString("dd-MM-yyyy"),
                IsCompleted = true,
                EInvoiceNumber = "PLN2026000000048", // invoice.EInvoiceNumber,
                ExpenseTypeCode = 1,
                OfficeCode = "M03", //invoice.OfficeCode,
                StoreCode = "M03", //invoice.StoreCode,
                Series = "",
                SeriesNumber = "0",
                VendorCode = "320.01.1222",//invoice.VendorCode,
                SubCurrAccID = "00000000-0000-0000-0000-000000000000",
                WarehouseCode = "M03",
                IsCreditSale = true,
                PaymentTerm = 30,
                Lines = new List<WholesaleInvoiceLine>
                {
                    new WholesaleInvoiceLine
                    {
                        UsedBarcode = "BAZ057",
                        PriceVI = 227.25M,
                        Qty1 = 10,
                    }
                }
            };

            return invoiceRequest;
        }

        //private static WholesaleInvoiceLine MapLine(InvoiceLines l) => new()
        //{
        //    ItemCode = l.ItemCode,
        //    ItemTypeCode = (int)l.ItemTypeCode,
        //    Qty1 = l.ItemQuantity,
        //    GLTypeCode = l.GLTypeCode,
        //    PriceVI = l.ItemTotalAmount,
        //    LDisRate1 = (int)l.DiscountRate * 100
        //};
    }

    public class RequestCreateWholesaleInvoice
    {
        public int ModelType { get; set; }
        public string CompanyCode { get; set; }
        public string InvoiceDate { get; set; }
        public bool IsCompleted { get; set; }
        public string EInvoiceNumber { get; set; }
        public int? ExpenseTypeCode { get; set; }
        public string OfficeCode { get; set; }
        public string Series { get; set; }
        public string SeriesNumber { get; set; }
        public string StoreCode { get; set; }
        public string VendorCode { get; set; }
        public string SubCurrAccID { get; set; }
        public string WarehouseCode { get; set; }
        public int PaymentTerm { get; set; }
        public bool IsCreditSale { get; set; }

        public List<WholesaleInvoiceLine> Lines { get; set; }                       //			                Satır (Dizi) 
    }

    public class WholesaleInvoiceLine
    {
        public string UsedBarcode { get; set; }
        public int Qty1 { get; set; }
        public decimal PriceVI { get; set; }

        public string ItemCode { get; set; }
        public string CostCenterCode { get; set; }
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
        public string? RejectionDesc { get; set; }
    }
}
