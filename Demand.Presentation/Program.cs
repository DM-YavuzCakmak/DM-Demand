using Demand.Business.Abstract.AuthorizationService;
using Demand.Business.Abstract.CompanyLocation;
using Demand.Business.Abstract.CompanyService;
using Demand.Business.Abstract.DemandMediaService;
using Demand.Business.Abstract.DemandService;
using Demand.Business.Abstract.Department;
using Demand.Business.Abstract.PersonnelService;
using Demand.Business.Concrete.AuthorizationService;
using Demand.Business.Concrete.CompanyLocation;
using Demand.Business.Concrete.CompanyService;
using Demand.Business.Concrete.DemandMediaService;
using Demand.Business.Concrete.DemandService;
using Demand.Business.Concrete.DepartmentService;
using Demand.Business.Concrete.PersonnelService;
using Demand.Domain.Entities.CompanyLocation;
using Demand.Infrastructure.DataAccess.Abstract.DemandMedia;
using Demand.Infrastructure.DataAccess.Abstract.Department;
using Demand.Infrastructure.DataAccess.Abstract.ICompanyLocationRepository;
using Demand.Infrastructure.DataAccess.Abstract.ICompanyRepository;
using Demand.Infrastructure.DataAccess.Abstract.IDemandRepository;
using Demand.Infrastructure.DataAccess.Abstract.Personnel;
using Demand.Infrastructure.DataAccess.Concrete.EntityFramework.CompanyLocationRepository;
using Demand.Infrastructure.DataAccess.Concrete.EntityFramework.CompanyRepository;
using Demand.Infrastructure.DataAccess.Concrete.EntityFramework.Contexts;
using Demand.Infrastructure.DataAccess.Concrete.EntityFramework.Demand;
using Demand.Infrastructure.DataAccess.Concrete.EntityFramework.DemandMedia;
using Demand.Infrastructure.DataAccess.Concrete.EntityFramework.Department;
using Demand.Infrastructure.DataAccess.Concrete.EntityFramework.Personnel;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

#region DbContext
builder.Services.AddDbContext<DemandContext>(options =>
{
    options.UseSqlServer("Data Source=172.30.47.17,1433;Initial Catalog=Demand;User Id=sa;Password=123456;TrustServerCertificate=true;");
});
#endregion

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

#region Demand
builder.Services.AddScoped<IDemandRepository, DemandRepository>();
builder.Services.AddScoped<IDemandService, DemandService>();
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


#endregion

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(o =>{});

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Login}/{id?}");

app.Run();
