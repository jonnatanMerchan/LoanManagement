using LoanManagement.Domain.Entities;
using LoanManagement.Domain.Enums;

namespace LoanManagement.Domain.Interfaces;

public interface ILoanRepository : IRepository<Loan>
{
    Task<IEnumerable<Loan>> GetByCustomerIdAsync(Guid customerId);
    Task<IEnumerable<Loan>> GetByStatusAsync(LoanStatus status);
    Task<(IEnumerable<Loan> Items, int TotalCount)> GetPagedAsync(
        int pageNumber, 
        int pageSize, 
        LoanStatus? status = null,
        Guid? customerId = null);
    Task<Loan?> GetWithPaymentsAsync(Guid id);
}