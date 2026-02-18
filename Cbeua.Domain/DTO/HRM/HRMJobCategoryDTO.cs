using Cbeua.Domain.Entities.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Domain.DTO.HRM
{
    public class HRMJobCategoryCreateUpdateDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public bool IsActive { get; set; } = false;
        public bool IsDeleted { get; set; } = false;

    }
    public class HRMJobCategoryDTO : BaseEntity
    {
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public List<AuditLogDTO> AuditLogs { get; set; } = new List<AuditLogDTO>();

    }
    public class HRMJobCategoryPaginationParams : BasePaginationParams
    {
        public string? Id {  get; set; }
        public string? Name { get; set; } 
        public string? Description { get; set; }
        public bool ShowDeleted { get; set; } = false;
        public bool ShowInactive { get; set; } = true;
    }
}
