using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cbeua.Domain.Interfaces.IRepositories;
using Cbeua.Domain.Interfaces.IServices;
using Cbeua.InfraCore.Data;
using Cbeua.InfraCore.External;
using Cbeua.InfraCore.Repositories;
using Cbeua.CORE.Repositories;
using Cbeua.Core.Repositories;


namespace Cbeua.InfraCore
{
    public static class InfraCoreServiceCollectionExtensions
    {
             public static IServiceCollection AddInfraCoreServiceCollectionExtensions(this IServiceCollection services, IConfiguration configuration)
        {
       
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // Register application-level services here
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            
            services.AddScoped<IAuditLogRepository, AuditLogRepository>();
            services.AddScoped<IAttachmentRepository, AttachmentRepository>();
            
            services.AddScoped<IExceptionLogRepository , ExceptionLogRepository>();
            services.AddScoped<IAuditRepository , AuditRepository>();
          
            services.AddScoped<IFinancialYearRepository  , FinancialYearRepository>();
          
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
           
            services.AddScoped<IManagingComiteeRepository, ManagingComiteeRepository>();
            services.AddScoped<IMainPageRepository, MainPageRepository>();
            services.AddScoped<IDayQuoteRepository, DayQuoteRepository>();
            services.AddScoped<IUserRoleRightRepository, UserRoleRightRepository>();
            services.AddScoped<IUserTypeRepository, UserTypeRepository>();
            services.AddScoped<IYearMasterRepository, YearMasterRepository>();
            services.AddScoped<IRefundContributionRepository, RefundContributionRepository>();
            services.AddScoped<ICircleStateRepository, CircleStateRepository>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
           services.AddScoped<ICircleRepository, CircleRepository>();
            services.AddScoped<IStateRepository, StateRepository>();
            services.AddScoped<IBranchRepository, BranchRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ITwilioSmsSender, TwilioSmsSender>();
           services.AddScoped<IDesignationRepository, DesignationRepository>();
            services.AddScoped<IStatusRepository, StatusRepository>();
            services.AddScoped<IMonthRepository, MonthRepository>();
            services.AddScoped<IMemberRepository, MemberRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IAccountDirectEntryRepository, AccountDirectEntryRepository>();
            services.AddScoped<IDeathClaimRepository, DeathClaimRepository>();
            services.AddScoped<ISupportTicketRepository, SupportTicketRepository>();
            //services.AddScoped<ICustomResponseBuilder, CustomResponseBuilder>();

            //// Add helpers or utilities
            //services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

            return services;
        }
    }
}
