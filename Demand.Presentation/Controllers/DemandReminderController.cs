using Demand.Business.Abstract.DemandOfferService;
using Demand.Business.Abstract.DemandProcessService;
using Demand.Business.Abstract.PersonnelRole;
using Demand.Business.Abstract.PersonnelService;
using Demand.Business.Abstract.RoleService;
using Demand.Core.Utilities.Email;
using Demand.Domain.DTO;
using Demand.Domain.Entities.DemandOfferEntity;
using Demand.Domain.Entities.Personnel;
using Demand.Domain.Entities.PersonnelRole;
using Demand.Domain.Entities.Role;
using Demand.Domain.Enums;
using Demand.Infrastructure.DataAccess.Concrete.EntityFramework.Contexts;
using DocumentFormat.OpenXml.InkML;
using Kep.Helpers.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Demand.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DemandReminderController : ControllerBase
    {
        private readonly IDemandProcessService _demandProcessService;
        private readonly DemandContext _dbContext;
        private readonly IPersonnelRoleService _personnelRoleService;
        private readonly IRoleService _roleService;
        private readonly IPersonnelService _personnelService;
        private readonly IDemandOfferService _demandOfferService;


        public DemandReminderController(IDemandProcessService demandProcessService, DemandContext dbContext, IPersonnelRoleService personnelRoleService, IRoleService roleService, IPersonnelService personnelService, IDemandOfferService demandOfferService)
        {
            _demandProcessService = demandProcessService;
            _dbContext = dbContext;
            _personnelRoleService = personnelRoleService;
            _roleService = roleService;
            _personnelService = personnelService;
            _demandOfferService = demandOfferService;
        }

        #region ReminderDemand
        [HttpGet("ReminderDemand")]
        public async Task<IActionResult> ReminderDemand()
        {
            var results = await _dbContext.DemandProcesses
            .FromSqlRaw("EXEC [dbo].[ps_GetReminderDemand]")
            .ToListAsync();

            var groupedResults = results.GroupBy(x => x.ManagerId).Select(g => new { ManagerId = g.Key, DemandIds = g.Select(x => x.DemandId).ToList() }).ToList();

            foreach (var group in groupedResults)
            {
                PersonnelEntity personnel = _personnelService.GetById(group.ManagerId).Data;
                PersonnelRoleEntity personnelRole = _personnelRoleService.GetList(x => x.PersonnelId == personnel.Id).Data.FirstOrDefault();

                if (personnel.IsNotNull() && personnelRole.IsNotNull())
                {
                    var emailBody = $"Merhabalar Sayın {personnel.FirstName} {personnel.LastName},<br/><br/>" +
                                    $"Onayınızı bekleyen {group.DemandIds.Count} tane talep vardır. Aşağıdaki linklerden talep detaylarını kontrol edebilirsiniz:<br/><br/>";
                    var demandLink = "";
                    foreach (var demandId in group.DemandIds)
                    {
                        DemandOfferEntity demandOffer = _demandOfferService.GetList(x => x.DemandId == demandId).Data.FirstOrDefault();
                        if (personnelRole.RoleId == (int)PersonnelRoleEnum.ITManager ||
                            personnelRole.RoleId == (int)PersonnelRoleEnum.OperasyonManager ||
                            personnelRole.RoleId == (int)PersonnelRoleEnum.MuhasebeManager ||
                            personnelRole.RoleId == (int)PersonnelRoleEnum.MimariManager ||
                            personnelRole.RoleId == (int)PersonnelRoleEnum.PazarlamaManager ||
                            personnelRole.RoleId == (int)PersonnelRoleEnum.HediyelikveGiftShopManager ||
                            personnelRole.RoleId == (int)PersonnelRoleEnum.InsanKaynaklarıManager)
                        {
                            if (demandOffer.IsNotNull())
                            {
                                demandLink = $"http://172.30.44.13:5734/api/Demands/DemandOfferDetail?DemandId={demandId}";
                            }
                            else
                            {
                                demandLink = $"http://172.30.44.13:5734/api/Demands?id={demandId}";
                            }
                        }
                        else if (personnelRole.RoleId == (int)PersonnelRoleEnum.SatınAlmaManager)
                        {
                            demandLink = $"http://172.30.44.13:5734/api/Demands/Edit/{demandId}";
                        }
                        else if (personnelRole.RoleId == (int)PersonnelRoleEnum.HeadOfManager)
                        {   
                            demandLink = $"http://172.30.44.13:5734/api/Demands/DemandOfferDetail?DemandId={demandId}";
                        }
                        emailBody += $"<a href='{demandLink}'>Talep {demandId} Detayları</a><br/>";
                    }
                    emailBody += "<br/>Saygılarımızla.";

                    EmailHelper.SendEmail(new List<string> { personnel.Email }, "Onayınızı Bekleyen Satın Alma Talepleri", emailBody);
                }
            }
            return Ok(results);
            #endregion             

        }
    }
}