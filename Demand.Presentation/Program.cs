using Demand.Business.Abstract.ApprovedSupplierService;
using Demand.Business.Abstract.AuthorizationService;
using Demand.Business.Abstract.CompanyLocation;
using Demand.Business.Abstract.CompanyLocationUnitsService;
using Demand.Business.Abstract.CompanyService;
using Demand.Business.Abstract.CurrencyTypeService;
using Demand.Business.Abstract.DemandMediaService;
using Demand.Business.Abstract.DemandOfferService;
using Demand.Business.Abstract.DemandProcessService;
using Demand.Business.Abstract.DemandService;
using Demand.Business.Abstract.Department;
using Demand.Business.Abstract.InvoiceService;
using Demand.Business.Abstract.OfferMediaService;
using Demand.Business.Abstract.OfferRequestService;
using Demand.Business.Abstract.PersonnelRole;
using Demand.Business.Abstract.PersonnelService;
using Demand.Business.Abstract.ProductCategoryService;
using Demand.Business.Abstract.Provider;
using Demand.Business.Abstract.RequestInfo;
using Demand.Business.Abstract.RoleService;
using Demand.Business.Concrete.ApprovedSupplierService;
using Demand.Business.Concrete.AuthorizationService;
using Demand.Business.Concrete.CompanyLocation;
using Demand.Business.Concrete.CompanyLocationUnitsService;
using Demand.Business.Concrete.CompanyService;
using Demand.Business.Concrete.CurrencyTypeService;
using Demand.Business.Concrete.DemandMediaService;
using Demand.Business.Concrete.DemandOfferService;
using Demand.Business.Concrete.DemandProcessService;
using Demand.Business.Concrete.DemandService;
using Demand.Business.Concrete.DepartmentService;
using Demand.Business.Concrete.Invoice;
using Demand.Business.Concrete.OfferMediaService;
using Demand.Business.Concrete.OfferRequestService;
using Demand.Business.Concrete.PersonnelRoleService;
using Demand.Business.Concrete.PersonnelService;
using Demand.Business.Concrete.ProductCategoryService;
using Demand.Business.Concrete.ProviderService;
using Demand.Business.Concrete.RequestInfo;
using Demand.Business.Concrete.RoleService;
using Demand.Domain.Entities.CompanyLocation;
using Demand.Infrastructure.DataAccess.Abstract.ApprovedSuplier;
using Demand.Infrastructure.DataAccess.Abstract.CompanyLocationUnits;
using Demand.Infrastructure.DataAccess.Abstract.CurrencyType;
using Demand.Infrastructure.DataAccess.Abstract.DemandMedia;
using Demand.Infrastructure.DataAccess.Abstract.DemandOffer;
using Demand.Infrastructure.DataAccess.Abstract.DemandProcess;
using Demand.Infrastructure.DataAccess.Abstract.Department;
using Demand.Infrastructure.DataAccess.Abstract.ICompanyLocationRepository;
using Demand.Infrastructure.DataAccess.Abstract.ICompanyRepository;
using Demand.Infrastructure.DataAccess.Abstract.IDemandRepository;
using Demand.Infrastructure.DataAccess.Abstract.Invoice;
using Demand.Infrastructure.DataAccess.Abstract.OfferMedia;
using Demand.Infrastructure.DataAccess.Abstract.OfferRequest;
using Demand.Infrastructure.DataAccess.Abstract.Personnel;
using Demand.Infrastructure.DataAccess.Abstract.PersonnelRole;
using Demand.Infrastructure.DataAccess.Abstract.ProductCategory;
using Demand.Infrastructure.DataAccess.Abstract.Provider;
using Demand.Infrastructure.DataAccess.Abstract.RequestInfo;
using Demand.Infrastructure.DataAccess.Abstract.Role;
using Demand.Infrastructure.DataAccess.Concrete.EntityFramework.ApprovedSupplier;
using Demand.Infrastructure.DataAccess.Concrete.EntityFramework.CompanyLocationRepository;
using Demand.Infrastructure.DataAccess.Concrete.EntityFramework.CompanyLocationUnits;
using Demand.Infrastructure.DataAccess.Concrete.EntityFramework.CompanyRepository;
using Demand.Infrastructure.DataAccess.Concrete.EntityFramework.Contexts;
using Demand.Infrastructure.DataAccess.Concrete.EntityFramework.CurrencyType;
using Demand.Infrastructure.DataAccess.Concrete.EntityFramework.Demand;
using Demand.Infrastructure.DataAccess.Concrete.EntityFramework.DemandMedia;
using Demand.Infrastructure.DataAccess.Concrete.EntityFramework.DemandOffer;
using Demand.Infrastructure.DataAccess.Concrete.EntityFramework.DemandProcess;
using Demand.Infrastructure.DataAccess.Concrete.EntityFramework.Department;
using Demand.Infrastructure.DataAccess.Concrete.EntityFramework.Invoice;
using Demand.Infrastructure.DataAccess.Concrete.EntityFramework.OfferMedia;
using Demand.Infrastructure.DataAccess.Concrete.EntityFramework.OfferRequest;
using Demand.Infrastructure.DataAccess.Concrete.EntityFramework.Personnel;
using Demand.Infrastructure.DataAccess.Concrete.EntityFramework.PersonnelRole;
using Demand.Infrastructure.DataAccess.Concrete.EntityFramework.ProductCategory;
using Demand.Infrastructure.DataAccess.Concrete.EntityFramework.Provider;
using Demand.Infrastructure.DataAccess.Concrete.EntityFramework.RequestInfo;
using Demand.Infrastructure.DataAccess.Concrete.EntityFramework.Role;
using Demand.Presentation.Services;
using Demand.Presentation.Utilities.Token;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

#region DbContext
builder.Services.AddDbContext<DemandContext>(options =>
{
    options.UseSqlServer("Data Source=172.30.196.15;Initial Catalog=Demand;User ID=sa;Password=HsMEfeS23@.;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;");
});
#endregion

builder.Services.AddHostedService<PendingDemandReminderWorker>();

#region Injection
builder.Services.AddHttpClient();

#region Company
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<ICompanyService, CompanyService>();
#endregion

#region CompanyLocation
builder.Services.AddScoped<ICompanyLocationRepository, CompanyLocationRepository>();
builder.Services.AddScoped<ICompanyLocationService, CompanyLocationService>();
#endregion

#region CompanyLocation
builder.Services.AddScoped<ICompanyLocationUnitsRepository, CompanyLocationUnitsRepository>();
builder.Services.AddScoped<ICompanyLocationUnitsService, CompanyLocationUnitsService>();
#endregion

#region Demand
builder.Services.AddScoped<IDemandRepository, DemandRepository>();
builder.Services.AddScoped<IDemandService, DemandService>();
#endregion

#region Invoice
builder.Services.AddScoped<IInvoiceDetailRepository, InvoiceDetailRepository>();
builder.Services.AddScoped<IInvoiceDetailService, InvoiceDetailService>();

builder.Services.AddScoped<IInvoiceProcessRepository, InvoiceProcessRepository>();
builder.Services.AddScoped<IInvoiceProcessService, InvoiceProcessService>();

builder.Services.AddScoped<IInvoiceDemandRepository, InvoiceDemandRepository>();
builder.Services.AddScoped<IInvoiceDemandService, InvoiceDemandService>();
#endregion

#region Personnel - Authorize
builder.Services.AddScoped<IPersonnelRepository, PersonnelRepository>();
builder.Services.AddScoped<IPersonnelService, PersonnelService>();
builder.Services.AddScoped<IAuthorizationService, AuthorizationService>();
#endregion

#region Department
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
#endregion

#region DemandMedia
builder.Services.AddScoped<IDemandMediaRepository, DemandMediaRepository>();
builder.Services.AddScoped<IDemandMediaService, DemandMediaService>();
#endregion

#region OfferMedia
builder.Services.AddScoped<IOfferMediaRepository, OfferMediaRepository>();
builder.Services.AddScoped<IOfferMediaService, OfferMediaService>();
#endregion

#region DemanProcess
builder.Services.AddScoped<IDemandProcessRepository, DemandProcessRepository>();
builder.Services.AddScoped<IDemandProcessService, DemandProcessService>();
#endregion

#region RequestInfo
builder.Services.AddScoped<IRequestInfoRepository, RequestInfoRepository>();
builder.Services.AddScoped<IRequestInfoService, RequestInfoService>();
#endregion

#region CurrencyType
builder.Services.AddScoped<ICurrencyTypeRepository, CurrencyTypeRepository>();
builder.Services.AddScoped<ICurrencyTypeService, CurrencyTypeService>();
#endregion


#region DemandOffer
builder.Services.AddScoped<IDemandOfferRepository, DemandOfferRepository>();
builder.Services.AddScoped<IDemandOfferService, DemandOfferService>();
#endregion


#region Provider
builder.Services.AddScoped<IProviderRepository, ProviderRepository>();
builder.Services.AddScoped<IProviderService, ProviderService>();
#endregion

#region ApprovedSupplier
builder.Services.AddScoped<IApprovedSupplierRepository, ApprovedSupplierRepository>();
builder.Services.AddScoped<IApprovedSupplierService, ApprovedSupplierService>();
#endregion


#region OfferRequest
builder.Services.AddScoped<IOfferRequestRepository, OfferRequestRepository>();
builder.Services.AddScoped<IOfferRequestService, OfferRequestService>();
#endregion

#region ProductCategory
builder.Services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
builder.Services.AddScoped<IProductCategoryService, ProductCategoryService>();
#endregion

#region Role
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IRoleService, RoleService>();
#endregion


#region PersonnelRole
builder.Services.AddScoped<IPersonnelRoleRepository, PersonnelRoleRepository>();
builder.Services.AddScoped<IPersonnelRoleService, PersonnelRoleService>();
#endregion


builder.Services.AddScoped<EMailService>();
builder.Services.AddScoped<TokenService>();

#endregion

var jwtSettings = builder.Configuration.GetSection("Jwt");

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
{
    options.LoginPath = "/Home/Login";
    options.AccessDeniedPath = "/Home/Login";
})
.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtSettings["Key"] ?? throw new Exception("JWT Key eksik"))),
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();

builder.Services.AddHttpContextAccessor();


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}



app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Login}/{id?}");

app.Run();
