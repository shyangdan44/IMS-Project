using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMS.infrastructure.Entity_Configuration;
using IMS.models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace IMS.infrastructure
{
    public class IMSDbContext:DbContext
    {

        public IMSDbContext(DbContextOptions<IMSDbContext> DbContext) : base(DbContext) 
        {
            
        }
        public DbSet<ReportDetailViewModel> ReportDetailViewModels { get; set; }
        public DbSet<UserInfo> UserInfos { get; set; }
        public DbSet<DashboardList> DashboardLists { get; set; }
        public DbSet<DashboardIndex> DashboardIndices { get; set; }
        public DbSet<CustomReportViewModel> CustomReportViewModels { get; set; }
        public DbSet<ProductViewModel> ProductViewModels { get; set; }
        protected override void OnModelCreating(ModelBuilder Builder)
        {
            Builder.ApplyConfiguration(new StoreConfiguration());
			Builder.ApplyConfiguration(new CategoryInfoConfiguration());
			Builder.ApplyConfiguration(new CustomerInfoConfiguration());
			Builder.ApplyConfiguration(new UnitInfoConfiguration());
			Builder.ApplyConfiguration(new ProductInfoConfiguration());
			Builder.ApplyConfiguration(new RackInfoConfiguration());
			Builder.ApplyConfiguration(new SupplierInfoConfiguration());
			Builder.ApplyConfiguration(new ProductRateInfoConfiguration());
			Builder.ApplyConfiguration(new StockInfoConfiguration());
			Builder.ApplyConfiguration(new TransactionInfoConfiguration());
			Builder.ApplyConfiguration(new ProductInvoiceInfoConfiguration());
			Builder.ApplyConfiguration(new ProductInvoiceDetailInfoConfiguration());
            Builder.Entity<CustomReportViewModel>().HasNoKey();
            Builder.Entity<ReportDetailViewModel>().HasNoKey();
            Builder.Entity<DashboardList>().HasNoKey();
            Builder.Entity<DashboardIndex>().HasNoKey();
        }


    }

 
}
