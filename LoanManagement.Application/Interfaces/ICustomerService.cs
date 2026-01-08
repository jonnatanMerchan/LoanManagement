using LoanManagement.Application.DTOs.Common;
using LoanManagement.Application.DTOs.Customer;

namespace LoanManagement.Application.Interfaces;

public interface ICustomerService
{
    Task<CustomerDto?> GetByIdAsync(Guid id);
    Task<PagedResult<CustomerDto>> GetPagedAsync(int pageNumber, int pageSize);
    Task<IEnumerable<CustomerDto>> SearchAsync(string searchTerm);
    Task<CustomerDto> CreateAsync(CreateCustomerDto dto);
    Task<CustomerDto> UpdateAsync(Guid id, UpdateCustomerDto dto);
    Task DeleteAsync(Guid id);
}