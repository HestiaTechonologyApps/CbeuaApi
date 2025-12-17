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
        public DbSet<DayQuote> DayQuotes { get; set; }
        public DbSet<UserRoleRight> UserRoleRights { get; set; }
        public DbSet<UserType> UserTypes { get; set; }
        public DbSet<YearMaster> YearMasters { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<CircleState> CircleStates { get; set; }
        public DbSet<FinancialYear> FinancialYears { get; set; }

        public DbSet<Accounts> Accounts { get; set; }
        public DbSet<AccountsDirectEntry> AccountsDirectEntries { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Circle> Circles { get; set; }
        public DbSet<ContributionMaster> ContributionMasters { get; set; }
        public DbSet<DeathClaim> DeathClaims { get; set; }
        public DbSet<Designation> Designations { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Month> Months { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<Status> statuses { get; set; }
        public DbSet<SupportTicket> SupportTickets { get; set; }
        public DbSet<ContributionDetail> ContributionDetails { get; set; }
        public DbSet<RefundContribution> RefundContributions { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<Comment> comments { get; set; }

        public DbSet<DailyNews> DailyNews { get; set; }


        public DbSet<AuditLog> AuditLogs { get; set; }


        public DbSet<ExceptionLog> ExceptionLogs { get; set; }
        public IEnumerable<object> TripOrder { get; set; }
        public DbSet<UserLoginLog> UserLoginLogs { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {




        }
    }
}
