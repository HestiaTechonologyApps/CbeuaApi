using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;
using Cbeua.Domain.Interfaces.IRepositories;
using Cbeua.Domain.Interfaces.IServices;
using Microsoft.AspNetCore.Rewrite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Bussiness.Services
{
    public class DirectPaymentService : IDirectPaymentService
    {
        private readonly IDirectPaymentRepository _repo;
        private readonly IAuditRepository _auditRepository;
        public String AuditTableName { get; set; } = "DIRECTPAYMENT";

        public DirectPaymentService(IDirectPaymentRepository repo, IAuditRepository auditRepository)
        {
            _repo = repo;
            _auditRepository = auditRepository;
        }

        public async Task<List<DirectPaymentDTO>> GetAllAsync()
        {
            return _repo.QueryableDirectPayments().ToList();
        }

        public async Task<DirectPaymentDTO?> GetByIdAsync(int id)
        {
            var q = _repo.QueryableDirectPayments();
            var directPayment = q.Where(dp => dp.DirectPaymentId == id).FirstOrDefault();

            return directPayment;
        }
        public async Task<List<DirectPaymentDTO>> GetByMemberId(int memberid)
        {
            var q = _repo.QueryableDirectPaymentsbyMemberId(memberid);
            var items =  q.Where(x => x.MemberId == memberid).ToList();
            return items;
        }

        public async Task<DirectPaymentDTO> CreateAsync(DirectPayment directPayment)
        {
            await _repo.AddAsync(directPayment);
            await _repo.SaveChangesAsync();
            await this._auditRepository.LogAuditAsync<DirectPayment>(
               tableName: AuditTableName,
               action: "create",
               recordId: directPayment.DirectPaymentId,
               oldEntity: null,
               newEntity: directPayment,
               changedBy: "System" // Replace with actual user info
            );
            return await ConvertDirectPaymentToDTO(directPayment);
        }
        public async Task<CustomApiResponse> ApproveAsync(int id, int currentUserId, bool approve)
        {
            try
            {
                var entry = await _repo.GetByIdAsync(id);
                if (entry == null || entry.IsDeleted)
                    return new CustomApiResponse
                    {
                        IsSucess = false,
                        Error = "Direct payment not found",
                        StatusCode = 404
                    };

                if (entry.isApproved)
                    return new CustomApiResponse
                    {
                        IsSucess = false,
                        Error = "Payment is already approved",
                        StatusCode = 400
                    };

                if (approve)
                {
                    var branchId = await _repo.GetBranchIdByMemberIdAsync(entry.MemberId);
                    if (branchId == 0)
                        return new CustomApiResponse
                        {
                            IsSucess = false,
                            Error = "Could not resolve BranchId for this member",
                            StatusCode = 400
                        };

                    var circleId = await _repo.GetCircleIdByMemberIdAsync(entry.MemberId);
                    if (circleId == 0)
                        return new CustomApiResponse
                        {
                            IsSucess = false,
                            Error = "Could not resolve CircleId for this member",
                            StatusCode = 400
                        };

                    var account = new Accounts
                    {
                        CircleId = circleId,
                        BranchId = branchId ?? 0,
                        MemeberId = entry.MemberId,
                        MonthCode = entry.PaymentDate.Month,
                        YearOf = entry.PaymentDate.Year,
                        Amount = entry.Amount,
                        TransMode = 10, 
                        Reference = entry.ReferenceNo,
                        Remark = string.IsNullOrWhiteSpace(entry.Remarks) ? "Direct Payment" : entry.Remarks
                    };

                    await _repo.AddAccountAsync(account);
                    entry.isApproved = true;
                    entry.ApprovedBy = currentUserId.ToString();
                    entry.ApprovedDate = DateTime.Now;
                }
                else
                {
                    entry.isApproved = false;
                    entry.ApprovedBy = currentUserId.ToString();
                    entry.ApprovedDate = DateTime.Now;
                }

                _repo.Update(entry);

                await _auditRepository.LogAuditAsync<DirectPayment>(
                    tableName: AuditTableName,
                    action: "update",
                    recordId: entry.DirectPaymentId,
                    oldEntity: CloneDirectPayment(entry),
                    newEntity: entry,
                    changedBy: currentUserId.ToString()
                );

                await _repo.SaveChangesAsync();

                return new CustomApiResponse
                {
                    IsSucess = true,
                    StatusCode = 200,
                    Value = approve
                        ? new { Message = "Direct payment approved and posted to accounts successfully" }
                        : new { Message = "Direct payment rejected successfully" }
                };
            }
            catch (Exception ex)
            {
                return new CustomApiResponse
                {
                    IsSucess = false,
                    Error = $"Exception: {ex.Message} | Inner: {ex.InnerException?.Message}",
                    StatusCode = 500
                };
            }
        }

        private async Task<DirectPaymentDTO> ConvertDirectPaymentToDTO(DirectPayment directPayment)
        {
            DirectPaymentDTO directPaymentDTO = new DirectPaymentDTO();
            directPaymentDTO.DirectPaymentId = directPayment.DirectPaymentId;
            directPaymentDTO.MemberId = directPayment.MemberId;
            directPaymentDTO.Amount = directPayment.Amount;
            directPaymentDTO.PaymentDate = directPayment.PaymentDate;
            directPaymentDTO.PaymentMode = directPayment.PaymentMode;
            directPaymentDTO.ReferenceNo = directPayment.ReferenceNo;
            directPaymentDTO.Remarks = directPayment.Remarks;
            directPaymentDTO.CreatedByUserId = directPayment.CreatedByUserId;
            directPaymentDTO.CreatedDate = directPayment.CreatedDate;
            directPaymentDTO.IsDeleted = directPayment.IsDeleted;
            return directPaymentDTO;
        }
        private DirectPayment CloneDirectPayment(DirectPayment entry)
        {
            return new DirectPayment
            {
                DirectPaymentId = entry.DirectPaymentId,
                MemberId = entry.MemberId,
                Amount = entry.Amount,
                PaymentDate = entry.PaymentDate,
                PaymentMode = entry.PaymentMode,
                ReferenceNo = entry.ReferenceNo,
                Remarks = entry.Remarks,
                CreatedByUserId = entry.CreatedByUserId,
                CreatedDate = entry.CreatedDate,
                IsDeleted = entry.IsDeleted,
                isApproved = entry.isApproved,
                ApprovedBy = entry.ApprovedBy,
                ApprovedDate = entry.ApprovedDate
            };
        }

        public async Task<bool> UpdateAsync(DirectPayment directPayment)
        {
            var oldentity = await _repo.GetByIdAsync(directPayment.DirectPaymentId);
            _repo.Detach(oldentity);
            _repo.Update(directPayment);
            await _repo.SaveChangesAsync();
            await _auditRepository.LogAuditAsync<DirectPayment>(
               tableName: AuditTableName,
               action: "update",
               recordId: directPayment.DirectPaymentId,
               oldEntity: oldentity,
               newEntity: directPayment,
               changedBy: "System" // Replace with actual user info
            );
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var directPayment = await _repo.GetByIdAsync(id);
            if (directPayment == null) return false;
            _repo.Delete(directPayment);
            await _auditRepository.LogAuditAsync<DirectPayment>(
               tableName: AuditTableName,
               action: "Delete",
               recordId: directPayment.DirectPaymentId,
               oldEntity: directPayment,
               newEntity: directPayment,
               changedBy: "System" // Replace with actual user info
            );
            await _repo.SaveChangesAsync();
            return true;
        }
    }
}