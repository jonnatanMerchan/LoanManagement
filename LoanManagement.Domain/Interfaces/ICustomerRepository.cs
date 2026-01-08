using LoanManagement.Domain.Entities;

namespace LoanManagement.Domain.Interfaces;

public interface ICustomerRepository : IRepository<Customer>
{
    Task<Customer?> GetByEmailAsync(string email);
    Task<IEnumerable<Customer>> SearchAsync(string searchTerm);
    Task<(IEnumerable<Customer> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize);
    Task<bool> EmailExistsAsync(string email, Guid? excludeId = null);
}