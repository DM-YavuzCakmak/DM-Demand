using Demand.Business.Abstract.Provider;
using Demand.Core.Attribute;
using Demand.Domain.DTO;
using Demand.Domain.Entities.ProviderEntity;
using Demand.Infrastructure.DataAccess.Concrete.EntityFramework.Contexts;
using DocumentFormat.OpenXml.Drawing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Data;
using static Demand.Presentation.Controllers.ProviderController;

namespace Demand.Presentation.Controllers
{
    [UserToken]
    public class ProviderController : Controller
    {
        private readonly DemandContext _dbContext;
        private readonly ILogger<InvoiceController> _logger;
        private readonly IProviderService _providerService;

        public ProviderController(ILogger<InvoiceController> logger, IProviderService providerService, DemandContext dbContext)
        {
            _logger = logger;
            _providerService = providerService;
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult New()
        {
            ViewData["ActivePage"] = "ProviderNew";

            return View();
        }

        public IActionResult Pending()
        {
            ViewData["ActivePage"] = "ProviderPending";

            var providerList = _providerService.GetList(x => x.IsDeleted && !x.IsApproved).Data;
            return View(providerList);
        }

        public IActionResult Approved()
        {
            ViewData["ActivePage"] = "ProviderApproved";

            var providerList = _providerService.GetList(x => !x.IsDeleted && x.IsApproved).Data;
            return View(providerList);
        }
        public class ProviderCreateViewModel
        {
            public string CompanyName { get; set; }
            public int CityId { get; set; }
            public int DistrictId { get; set; }
            public int IdentityType { get; set; } // 1:vergi | 2:tckn
            public string TaxNumber { get; set; }
            public string TaxAdministrationId { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
            public string AddressLine { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProviderCreateViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return Json(new { success = false, message = "Model geçersiz." });

                var provider = new ProviderEntity
                {
                    Name = model.CompanyName,
                    CityId = model.CityId,
                    DistrictId = model.DistrictId,
                    TaxNumber = model.IdentityType == 1 ? model.TaxNumber : null,
                    NationalIdNumber = model.IdentityType == 2 ? model.TaxNumber : null,
                    TaxOfficeCode = model.TaxAdministrationId,
                    Email = model.Email,
                    PhoneNumber = model.Phone,
                    Address = model.AddressLine,
                    CreatedDate = DateTime.Now,
                    IsDeleted = true,
                    IsApproved = false
                };
                _providerService.Add(provider);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult GetDetail(long id)
        {
            var supplier = _providerService.GetById(id).Data;

            if (supplier == null)
                return Json(new { success = false, message = "Tedarikçi bulunamadı." });

            if (supplier.CityId != null)
            {
                var province = ReadFromResponse<Provinces>(HttpGet("http://172.30.44.13:5316/Region/GetProvince?provinceId=" + supplier.CityId));
                supplier.CityName = province?.Name;
                if (supplier.DistrictId != null)
                {
                    var district = ReadFromResponse<List<Citys>>(HttpGet("http://172.30.44.13:5316/Region/GetStates?cityId=" + supplier.CityId)).FirstOrDefault(x => x.Id == supplier.DistrictId);
                    supplier.DistrictName = district?.Name;
                }
                if (supplier.TaxOfficeCode != null)
                {
                    var taxOffice = GetTaxOfficeOptionsAsync(supplier.CityId.Value).Result.FirstOrDefault(x => x.TaxOfficeCode == supplier.TaxOfficeCode);
                    supplier.TaxOfficeName = taxOffice?.TaxOfficeDescription;
                }
            }
            return Json(new { success = true, data = supplier });
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetProvinces()
        {
            try
            {
                var ret = ReadFromResponse<List<Provinces>>(HttpGet("http://172.30.44.13:5316/Region/GetCities?countryId=178"));
                return Json(ret);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetCities(int provinceId)
        {
            try
            {
                var ret = ReadFromResponse<List<Citys>>(HttpGet("http://172.30.44.13:5316/Region/GetStates?cityId=" + provinceId));
                return Json(ret);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static T ReadFromResponse<T>(string response)
        {
            try
            {
                if (response is null)
                    return default(T);

                if (response.ToLower() == "false")
                    return default(T);

                return JsonConvert.DeserializeObject<T>(response.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string HttpGet(string path)
        {
            HttpClient client = new();
            try
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("X-RPC-AUTHORIZATION", "hagiasophia:hagiasophia@123");
                client.DefaultRequestHeaders.Add("X-RPC-DIRECTORY", "main");
                var value = client.GetAsync(path).Result;
                if (value.IsSuccessStatusCode)
                {
                    return value.Content.ReadAsStringAsync().Result;
                }
                return string.Empty;
            }
            catch (HttpRequestException ex)
            {
                //Log.Fatal(ex, null);
                throw ex;
            }
        }

        public class TaxOfficeInfo
        {
            public string TaxOfficeCode { get; set; }
            public string TaxOfficeDescription { get; set; }
            public string CityCode { get; set; }
            public string CityDescription { get; set; }
        }

        public class Provinces
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int CountryId { get; set; }
            public string? NebimCityCode { get; set; }
        }

        public class Citys
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int ProvinceId { get; set; }
            public string? NebimStateCode { get; set; }
            public string? NebimDistrictCode { get; set; }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetTaxAdministration(int provinceId)
        {
            return Json(GetTaxOfficeOptionsAsync(provinceId));
        }

        public async Task<List<TaxOfficeInfo>> GetTaxOfficeOptionsAsync(int provinceId)
        {

            var connectionString = "Data Source=172.30.196.11;Initial Catalog=Dem_V3;User Id=sa;Password=Asist@1489;TrustServerCertificate=true;";
            var query = @"
        SELECT cdTaxOffice.TaxOfficeCode
              , TaxOfficeDescription
              , cdCityDesc.CityCode
              , CityDescription
        FROM dbo.cdTaxOffice WITH (NOLOCK)
        INNER JOIN dbo.cdTaxOfficeDesc WITH (NOLOCK)
            ON cdTaxOfficeDesc.TaxOfficeCode = cdTaxOffice.TaxOfficeCode
            AND cdTaxOfficeDesc.LangCode = 'TR'
        INNER JOIN dbo.cdCityDesc WITH (NOLOCK)
            ON cdCityDesc.CityCode = cdTaxOffice.CityCode
            AND cdCityDesc.LangCode = 'TR'
            AND cdCityDesc.CityCode = @ProvinceCode
        ORDER BY TaxOfficeDescription;";

            var results = new List<TaxOfficeInfo>();

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand(query, connection))
                {
                    var nebimCityCode = await GetNebimCityCode(provinceId);
                    command.Parameters.Add("@ProvinceCode", SqlDbType.NVarChar).Value = nebimCityCode;

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var taxOfficeInfo = new TaxOfficeInfo
                            {
                                TaxOfficeCode = reader.GetString(reader.GetOrdinal("TaxOfficeCode")),
                                TaxOfficeDescription = reader.GetString(reader.GetOrdinal("TaxOfficeDescription")),
                                CityCode = reader.GetString(reader.GetOrdinal("CityCode")),
                                CityDescription = reader.GetString(reader.GetOrdinal("CityDescription"))
                            };

                            results.Add(taxOfficeInfo);
                        }
                    }
                }
            }

            return results;
        }

        [HttpGet("GetNebimCityCode")]
        private async Task<string> GetNebimCityCode(int provinceId)
        {
            var provinces = ReadFromResponse<Provinces>(HttpGet($"http://172.30.44.13:5316/Region/GetProvince?provinceId={provinceId}"));
            return provinces?.NebimCityCode ?? string.Empty;
        }
    }
}
