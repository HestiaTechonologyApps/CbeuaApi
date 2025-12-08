using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;
using Cbeua.Domain.Entities;
using Cbeua.Domain.Entities.Common;


namespace Cbeua.InfraCore.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // Public Users

        public DbSet<Company> Companies { get; set; }

        public DbSet<ManagingComitee> ManagingComitees { get; set; }

        public DbSet<MainPage> MainPages { get; set; }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<User> Users { get; set; }

        public DbSet<FinancialYear> FinancialYears { get; set; }

     

        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<Comment> comments { get; set; }




        public DbSet<AuditLog> AuditLogs { get; set; }


        public DbSet<ExceptionLog> ExceptionLogs { get; set; }
        public IEnumerable<object> TripOrder { get; set; }
        public DbSet<UserLoginLog> UserLoginLogs { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {




        }
    }
}
