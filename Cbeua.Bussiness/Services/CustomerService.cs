using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cbeua.Domain.Interfaces.IRepositories;
using Cbeua.Domain.Interfaces.IServices;
using Cbeua.Domain.Interfaces.IServices;
using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;
using Cbeua.Domain.DTO;
using Microsoft.EntityFrameworkCore;

namespace Cbeua.Bussiness.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repo;
        private readonly IAuditRepository _auditRepository;
        private readonly ICurrentUserService _currentUser;
        public String AuditTableName { get; set; } = "CUSTOMER";
        public CustomerService(ICustomerRepository repo , IAuditRepository auditRepository, ICurrentUserService currentUser)
        {
            _repo = repo;

            this._auditRepository = auditRepository;
            _currentUser = currentUser;
        }
        public async Task<CustomerDTO> CreateAsync(Customer customer)
        {
            customer.CompanyId = int.Parse(_currentUser.CompanyId);
            await _repo.AddAsync(customer);
            await _repo.SaveChangesAsync();
            await this._auditRepository.LogAuditAsync<Customer>(
                tableName: AuditTableName,
                action: "create",
                recordId: customer.CustomerId,
                oldEntity: null,
                newEntity: customer,
                changedBy: _currentUser.Email.ToString() // Replace with actual user info
            );
            return await ConvertCustomerToDTO(customer);
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var customer = await _repo.GetByIdAsync(id);

            if (customer == null) return false;
            _repo.Delete(customer);
            await _repo.SaveChangesAsync();
            await _auditRepository.LogAuditAsync<Customer>(
               tableName: AuditTableName,
               action: "Delete",
               recordId: customer.CustomerId,
               oldEntity: customer,
               newEntity: customer,
               changedBy: _currentUser.Email.ToString() // Replace with actual user info
           );
            return true;
        }
        public async Task<List<CustomerDTO>> GetAllAsync()
        {


           // List<CustomerDTO> customerdtos = new List<CustomerDTO>();

            var customers = await _repo.GetQuerableCustomerList();

            //foreach (var driver in customers)
            //{
            //    CustomerDTO customerDTO = await ConvertCustomerToDTO(driver);
            //    customerdtos.Add(customerDTO);


            //}

            return await customers.ToListAsync ();
        }

        public async Task<CustomerDTO?> GetByIdAsync(int id)
        {
            var q = await _repo.GetByIdAsync(id);
            if (q == null) return null;
            var customerdto = await ConvertCustomerToDTO(q);
           // customerdto.AuditTrails = await _auditRepository.GetAuditLogsForEntityAsync("Customers", customerdto.CustomerId);
            return customerdto;
        }

        public async Task<bool> UpdateAsync(Customer customer)
        {
            var oldentity = await _repo.GetByIdAsync(customer.CustomerId);
            _repo.Detach(oldentity);
            _repo.Update(customer);
            customer.CompanyId = int.Parse(_currentUser.CompanyId);
            await _repo.SaveChangesAsync();
            await _auditRepository.LogAuditAsync<Customer>(
              tableName: AuditTableName,
              action: "update",
              recordId: customer.CustomerId,
              oldEntity: oldentity,
              newEntity: customer,
              changedBy: _currentUser.Email.ToString() // Replace with actual user info
          );
            return true;
        }

        private async  Task<CustomerDTO> ConvertCustomerToDTO(Customer customer)
        {
            CustomerDTO customerDTO = new CustomerDTO();
            customerDTO.CustomerId = customer.CustomerId;
            customerDTO.CustomerName = customer.CustomerName;
            customerDTO.CustomerPhone = customer.CustomerPhone;
            customerDTO.CustomerEmail = customer.CustomerEmail;
            customerDTO.CustomerAddress = customer.CustomerAddress;
            customerDTO.DOB = customer.DOB;
            customerDTO.DOBString= customer.DOB.ToString("dd MMMM yyyy hh:mm tt");
            
            customerDTO.Nationality = customer.Nationalilty;
            customerDTO.IsActive = customer.IsActive;
            customerDTO.CreatedAt = customer.CreatedAt;
            customerDTO.CreateAtString = customer.CreatedAt.ToString("dd MMMM yyyy hh:mm tt");
            customerDTO.CompanyId = customer.CompanyId;
            //customerDTO.AuditTrails = await _auditRepository.GetAuditLogsForEntityAsync("Customer",customer.CustomerId );
            return customerDTO;
        }

    }


}