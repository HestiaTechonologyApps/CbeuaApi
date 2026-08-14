using Cbeua.Domain.DTO;
using Cbeua.Domain.Interfaces.IRepositories;
using Cbeua.Domain.Interfaces.IServices;
using System.Linq;
using System.Threading.Tasks;

namespace Cbeua.Bussiness.Services
{
    public class LookupService : ILookupService
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IBranchRepository _branchRepository;
        private readonly IExpenseTypeRepository _expenseTypeRepository;

        public LookupService(IMemberRepository memberRepository, IBranchRepository branchRepository, IExpenseTypeRepository expenseTypeRepository)
        {
            _memberRepository = memberRepository;
            _branchRepository = branchRepository;
            _expenseTypeRepository = expenseTypeRepository;
        }

        public async Task<CustomApiResponse> GetPagedLookupAsync(LookupPaginationParams parameters)
        {
            var entityName = parameters.EntityName?.Trim().ToLowerInvariant();

            return entityName switch
            {
                "member" => GetMemberLookup(parameters),
                "branch" => GetBranchLookup(parameters),
                "expensetype" => GetExpenseTypeLookup(parameters),
                _ => new CustomApiResponse
                {
                    IsSucess = false,
                    Error = $"Unknown entity: {parameters.EntityName}",
                    StatusCode = 400
                }
            };
        }

        private CustomApiResponse GetMemberLookup(LookupPaginationParams p)
        {
            var pageNumber = p.PageNumber <= 0 ? 1 : p.PageNumber;
            var pageSize = p.PageSize <= 0 ? 10 : p.PageSize;

            var query = _memberRepository.GetMemberLookup(p.LookupMasterId);

            if (!string.IsNullOrWhiteSpace(p.SearchTerm))
            {
                var s = p.SearchTerm.Trim().ToLower();
                query = query.Where(x =>
                    x.MemberName.ToLower().Contains(s) ||
                    x.StaffNo.ToString().Contains(s));
            }

            var all = query.OrderBy(x => x.MemberName).ToList();
            var total = all.Count;

            var items = all.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            foreach (var i in items)
                i.IsSelected = p.SelectedId.HasValue && i.MemberId == p.SelectedId.Value;

            if (p.SelectedId.HasValue && items.All(x => x.MemberId != p.SelectedId.Value))
            {
                var selected = all.FirstOrDefault(x => x.MemberId == p.SelectedId.Value);
                if (selected != null)
                {
                    selected.IsSelected = true;
                    items.Insert(0, selected);
                }
            }

            return new CustomApiResponse
            {
                IsSucess = true,
                StatusCode = 200,
                Value = new PaginatedResult<MemberLookupDTO> { Data = items, Total = total }
            };
        }

        private CustomApiResponse GetBranchLookup(LookupPaginationParams p)
        {
            var pageNumber = p.PageNumber <= 0 ? 1 : p.PageNumber;
            var pageSize = p.PageSize <= 0 ? 10 : p.PageSize;

            var query = _branchRepository.GetBranchLookup();

            if (!string.IsNullOrWhiteSpace(p.SearchTerm))
            {
                var s = p.SearchTerm.Trim().ToLower();
                query = query.Where(x =>
                    x.BranchName.ToLower().Contains(s) ||
                    x.DpCode.Contains(s));
            }

            var all = query.OrderBy(x => x.BranchName).ToList();
            var total = all.Count;

            var items = all.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            foreach (var i in items)
                i.IsSelected = p.SelectedId.HasValue && i.BranchId == p.SelectedId.Value;

            if (p.SelectedId.HasValue && items.All(x => x.BranchId != p.SelectedId.Value))
            {
                var selected = all.FirstOrDefault(x => x.BranchId == p.SelectedId.Value);
                if (selected != null)
                {
                    selected.IsSelected = true;
                    items.Insert(0, selected);
                }
            }

            return new CustomApiResponse
            {
                IsSucess = true,
                StatusCode = 200,
                Value = new PaginatedResult<BranchLookupDTO> { Data = items, Total = total }
            };
        }
        private CustomApiResponse GetExpenseTypeLookup(LookupPaginationParams p)
        {
            var pageNumber = p.PageNumber <= 0 ? 1 : p.PageNumber;
            var pageSize = p.PageSize <= 0 ? 10 : p.PageSize;

            var query = _expenseTypeRepository.GetExpenseLookup();

            if (!string.IsNullOrWhiteSpace(p.SearchTerm))
            {
                var s = p.SearchTerm.Trim().ToLower();
                query = query.Where(x =>
                    x.Name.ToLower().Contains(s));
                  
            }

            var all = query.OrderBy(x => x.Name).ToList();
            var total = all.Count;

            var items = all.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            foreach (var i in items)
                i.IsSelected = p.SelectedId.HasValue && i.ExpenseTypeId == p.SelectedId.Value;

            if (p.SelectedId.HasValue && items.All(x => x.ExpenseTypeId != p.SelectedId.Value))
            {
                var selected = all.FirstOrDefault(x => x.ExpenseTypeId == p.SelectedId.Value);
                if (selected != null)
                {
                    selected.IsSelected = true;
                    items.Insert(0, selected);
                }
            }

            return new CustomApiResponse
            {
                IsSucess = true,
                StatusCode = 200,
                Value = new PaginatedResult<ExpenseLookupDTO> { Data = items, Total = total }
            };
        }
    }
}