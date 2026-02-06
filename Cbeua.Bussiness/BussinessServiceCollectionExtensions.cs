using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cbeua.Business.Services;
using Cbeua.Bussiness.Services;
using Cbeua.Domain.Configurations;
using Cbeua.Domain.Interfaces.IServices;




namespace Cbeua.Bussiness
{
   public static class BussinessServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddScoped<IJwtService, JwtService>();            
           
            services.AddScoped<IAuditLogService, AuditLogService>();
           
           
           
            services.AddScoped<IExceptionLogService, ExceptionLogService>();
          
            
            services.AddScoped<IFinancialYearService, FinancialYearService>();
            services.AddScoped<ICompanyService, CompanyService>();
           
         
           
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IUserService, UserService>();
           
           
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IAttachmentService, AttachmentService>();
           
            services.AddScoped<IListService, ListService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IManagingComiteeService, ManagingComiteeService>();
            services.AddScoped<IMainPageService, MainPageService>();
            services.AddScoped<IDayQuoteService, DayQuoteService>();
            services.AddScoped<IUserRoleRightService, UserRoleRightService>();
            services.AddScoped<IUserTypeService, UserTypeService>();
            services.AddScoped<IYearMasterService, YearMasterService>();
            services.AddScoped<ICircleStateService, CircleStateService>();
            services.AddScoped<IRefundContributionService, RefundContributionService>();
            services.AddScoped<ICircleService, CircleService>();
            services.AddScoped<IStateService, StateService>();
            services.AddScoped<IBranchService, BranchService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IDesignationsService, DesignationService>();
            services.AddScoped<IStatusService, StatusService>();
            services.AddScoped<IMonthService, MonthService>();
            services.AddScoped<IMemberService, MemberService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IAccountDirectEntryService, AccountDirectEntryService>();
            services.AddScoped<IDeathClaimService, DeathClaimService>();
            services.AddScoped<ISupportTicketService, SupportTicketService>();
            services.AddScoped<IDailyNewsService, DailyNewsService>();
            services.AddScoped<IDirectPaymentService, DirectPaymentService>();
            services.AddScoped<IPublicPageService, PublicPageService>();
            services.AddScoped<IMonthlyContributionService, MonthlyContributionService>();
            services.AddScoped<IContactMessageService, ContactMessageService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<IReportTypeService, ReportTypeService>();
            //services.Configure<OtpSettings>(configuration.GetSection("OtpSettings"));
            //services.Configure<WalletSettings>(configuration.GetSection("Wallet"));

            // Add helpers or utilities
            //services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

            return services;
        }
    }
}
