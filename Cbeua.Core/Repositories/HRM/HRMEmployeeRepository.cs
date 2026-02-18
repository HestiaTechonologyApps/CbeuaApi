using Cbeua.Domain.DTO.HRMS;
using Cbeua.Domain.Entities.HRMS;
using Cbeua.Domain.Interfaces.IRepositories.HRMS;
using Cbeua.InfraCore.Data;

namespace Cbeua.Core.Repositories.HRMS
{
    public class HRMEmployeeRepository : GenericRepository<HRMEmployee>, IHRMEmployeeRepository
    {
        private readonly AppDbContext _context;

        public HRMEmployeeRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public Task<IQueryable<HRMEmployeeDTO>> GetQuerableList()
        {
            var q = (from emp in _context.HRMEmployees
                     join branch in _context.HRMBranches on emp.BranchId equals branch.Id
                     join dept in _context.HRMDepartments on emp.DepartmentId equals dept.Id
                     join desig in _context.HRMDesignations on emp.DesignationId equals desig.Id
                     select new HRMEmployeeDTO
                     {
                         Id = emp.Id,
                         FullName = emp.FullName,
                         EmployeeId = emp.EmployeeId,
                         EmployeeCode = emp.EmployeeCode,
                         Email = emp.Email,
                         PhoneNumber = emp.PhoneNumber,
                         DateOfBirth = emp.DateOfBirth,
                         Gender = emp.Gender,
                         ProfileImagePath = emp.ProfileImagePath,

                         BranchId = emp.BranchId,
                         BranchName = branch.Name,
                         DepartmentId = emp.DepartmentId,
                         DepartmentName = dept.Name,
                         DesignationId = emp.DesignationId,
                         DesignationName = desig.Name,
                         DateOfJoining = emp.DateOfJoining,
                         EmploymentType = emp.EmploymentType,
                         EmployeeStatus = emp.EmployeeStatus,
                         ShiftId = emp.ShiftId,
                         AttendancePolicyId = emp.AttendancePolicyId,

                         AddressLine1 = emp.AddressLine1,
                         AddressLine2 = emp.AddressLine2,
                         City = emp.City,
                         State = emp.State,
                         Country = emp.Country,
                         PostalCode = emp.PostalCode,

                         EmergencyContactName = emp.EmergencyContactName,
                         EmergencyContactRelationship = emp.EmergencyContactRelationship,

                         IsActive = emp.IsActive,
                         IsDeleted = emp.IsDeleted,
                         CreatedAt = emp.CreatedAt,
                         UpdatedAt = emp.UpdatedAt
                     }).AsQueryable();

            return Task.FromResult(q);
        }
    }
}