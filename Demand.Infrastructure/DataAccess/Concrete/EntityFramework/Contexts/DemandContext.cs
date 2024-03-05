using Demand.Domain.Entities.Company;
using Demand.Domain.Entities.CompanyLocation;
using Demand.Domain.Entities.Demand;
using Demand.Domain.Entities.DemandMediaEntity;
using Demand.Domain.Entities.Personnel;
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
        public virtual DbSet<DemandMediaEntity> DemandMedia { get; set; }
    }
}
