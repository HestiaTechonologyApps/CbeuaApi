using Microsoft.EntityFrameworkCore;
using Cbeua.Domain.DTO;
using Cbeua.Domain.Interfaces.IRepositories;
using Cbeua.Domain.Interfaces.IServices;


namespace Cbeua.Bussiness.Services
{
    public class ListService : IListService
    {
       
        private readonly ICurrentUserService _currentUser;
        public ListService( ICurrentUserService currentUser)
        {
           
            _currentUser = currentUser;
        }
        //public async Task<PaginatedResult<TripListDataDTO>> Get_PaginatedTripList(
        //  string listType,
        //  string filtertext,
        //  int pagesize,
        //  int pagenumber,
        //  int CustomerId,
        //  int Year)
        //{
        //    // Normalize pagination
        //    pagesize = Math.Max(1, Math.Min(pagesize, 100));
        //    pagenumber = Math.Max(pagenumber, 1);

        //    var q = _repo.QuerableTripListAsyc();  // Fixed method name

        //    // Filter by Customer
        //    if (CustomerId > 0)
        //    {
        //        q = q.Where(t => t.CustomerId == CustomerId);
        //    }

        //    // Filter by Year
        //    if (Year > 0)
        //    {
        //        q = q.Where(t => t.FromDate != null && t.FromDate.Value.Year == Year);
        //    }

        //    // Filter by List Type
        //    if (!string.IsNullOrWhiteSpace(listType))
        //    {
        //        var lowerListType = listType.ToLower();

        //        switch (lowerListType)
        //        {
        //            case "completed":
        //                q = q.Where(t => t.Status.ToLower() == "completed");
        //                break;

        //            case "scheduled":
        //                q = q.Where(t => t.Status.ToLower() == "scheduled");
        //                break;

        //            case "canceled":
        //                q = q.Where(t => t.Status.ToLower() == "canceled");
        //                break;
        //            case "ongoing":
        //                q = q.Where(t => t.Status.ToLower() == "started");
        //                break;


        //            case "upcoming":
        //                var now = DateTime.Now;
        //                var fromTime = now.AddHours(-3);      // 3 hours ago
        //                var toTime = now.AddHours(24);        // 24 hours from now

        //                q = q.Where(t =>
        //                    t.VehicleTakeOfTime >= fromTime &&
        //                    t.VehicleTakeOfTime <= toTime && t.Status.ToLower() != "scheduled"
        //                );
        //                break;
        //            case "today":
        //                var today = DateTime.Today;          
        //                var tomorrow = today.AddDays(1);

        //                q = q.Where(t =>
        //                    t.FromDate.HasValue &&
        //                    t.ToDate.HasValue &&
        //                    t.FromDate.Value < tomorrow &&
        //                    t.ToDate.Value >= today
        //                );
        //                break;


        //            case "uninvoiced":
        //                q = q.Where(t => t.Status.ToLower() == "completed" && t.IsInvoiced == false);
        //                break;

        //            case "all":
        //            default:
        //                // No additional filtering
        //                break;
        //        }
        //    }

        //    // Text filtering
        //    if (!string.IsNullOrWhiteSpace(filtertext))
        //    {
        //        var filter = filtertext.ToLower();
        //        q = q.Where(t =>
        //            t.CustomerName.ToLower().Contains(filter) ||
        //            t.DriverName.ToLower().Contains(filter) ||
        //            t.PickUpFrom.ToLower().Contains(filter) ||
        //            t.RecivedVia.ToLower().Contains(filter) ||
        //            t.Status.ToLower().Contains(filter) ||
        //            t.TripCode.ToLower().Contains(filter)
        //        );
        //    }

        //    // Get total count
        //    var total = await q.CountAsync();

        //    // Apply ordering and pagination
        //    var data = await q
        //        .OrderByDescending(t => t.FromDate)  // Default ordering by date
        //        .ThenBy(t => t.TripOrderId)           // Secondary sort for consistency
        //        .Skip((pagenumber - 1) * pagesize)
        //        .Take(pagesize)
        //        .ToListAsync();

        //    return new PaginatedResult<TripListDataDTO>
        //    {
        //        Total = total,
        //        Data = data.OrderBy(u => u.VehicleTakeOfTime)
        //    };
        //}


    }
}
