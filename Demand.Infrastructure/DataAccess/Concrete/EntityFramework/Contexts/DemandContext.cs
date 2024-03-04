using Demand.Domain.Entities.Company;
using Demand.Domain.Entities.CompanyLocation;
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


        public virtual DbSet<CompanyEntity> Companies { get; set; }
        public virtual DbSet<CompanyLocationEntity> CompanyLocations { get; set; }
    }
}
