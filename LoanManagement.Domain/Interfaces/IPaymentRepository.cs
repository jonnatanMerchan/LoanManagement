using LoanManagement.Domain.Entities;

namespace LoanManagement.Domain.Interfaces;

public interface IPaymentRepository : IRepository<Payment>
{
    Task<IEnumerable<Payment>> GetByLoanIdAsync(Guid loanId);
    Task<decimal> GetTotalPaidByLoanIdAsync(Guid loanId);
    Task<Payment?> GetByTransactionReferenceAsync(string reference);
}