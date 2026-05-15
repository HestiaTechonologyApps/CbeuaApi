using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;
using Cbeua.Domain.Interfaces.IRepositories;
using Cbeua.Domain.Interfaces.IServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Bussiness.Services
{
    public class MemberAccountDetailsService: IMemberAccountDetailsService
    {
        private readonly IMemberAccountDetailsRepository _repo;

        public MemberAccountDetailsService(IMemberAccountDetailsRepository repo)
        {
            _repo = repo;
        }
        public async Task<CustomApiResponse> GetByIdAsync(int memberId)
        {
            try
            {
                var details = await _repo.QueryableUserList(memberId).ToListAsync();
                if (!details.Any())
                    return new CustomApiResponse
                    {
                        IsSucess = false,
                        Error = "Contribution detail not found",
                        StatusCode = 404
                    };

                return new CustomApiResponse
                {
                    IsSucess = true,
                    StatusCode = 200,
                    Value = details
                };
            }
            catch (Exception ex)
            {
                return new CustomApiResponse
                {
                    IsSucess = false,
                    Error = $"Exception: {ex.Message}",
                    StatusCode = 500
                };
            }
        }
    }
}
