using Demand.Domain.Entities.ApprovedSupplierEntity;
using Demand.Domain.Entities.Company;
using Demand.Domain.Entities.CompanyLocation;
using Demand.Domain.Entities.CurrencyTypeEntity;
using Demand.Domain.Entities.Demand;
using Demand.Domain.Entities.DemandMediaEntity;
using Demand.Domain.Entities.DemandOfferEntity;
using Demand.Domain.Entities.DemandProcess;
using Demand.Domain.Entities.DepartmentEntity;
using Demand.Domain.Entities.Personnel;
using Demand.Domain.Entities.PersonnelRole;
using Demand.Domain.Entities.ProviderEntity;
using Demand.Domain.Entities.RequestInfoEntity;
using Demand.Domain.Entities.Role;
using Microsoft.EntityFrameworkCore;

namespace Demand.Infrastructure.DataAccess.Concrete.EntityFramework.Contexts
{
    public class DemandContext : DbContext
    {
        public DemandContext()
        {
        }

        public DemandContext(DbContextOptions<DemandContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=172.30.47.17,1433;Initial Catalog=Demand;User Id=sa;Password=123456;TrustServerCertificate=true;");
        }
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<PersonnelEntity> Personnel { get; set; }
        public virtual DbSet<CompanyLocation> CompanyLocations { get; set; }
        public virtual DbSet<DemandEntity> Demands { get; set; }
        public virtual DbSet<DemandMediaEntity> DemandMedias { get; set; }
        public virtual DbSet<DemandProcessEntity> DemandProcesses { get; set; }
        public virtual DbSet<DepartmentEntity> Departments { get; set; }
        public virtual DbSet<PersonnelRoleEntity> PersonnelRoles { get; set; }
        public virtual DbSet<RoleEntity> Roles { get; set; }
        public virtual DbSet<RequestInfoEntity> RequestInfos { get; set; }
        public virtual DbSet<CurrencyTypeEntity> CurrencyTypes { get; set; }
        public virtual DbSet<DemandOfferEntity> DemandOffers { get; set; }
        public virtual DbSet<ProviderEntity> Providers { get; set; }
        public virtual DbSet<ApprovedSupplierEntity> ApprovedSuppliers { get; set; }

    }
}
