using Microsoft.EntityFrameworkCore;
using LoanManagement.Domain.Entities;
using LoanManagement.Domain.Interfaces;
using LoanManagement.Infrastructure.Data;

namespace LoanManagement.Infrastructure.Repositories;

public class PaymentRepository : Repository<Payment>, IPaymentRepository
{
    public PaymentRepository(LoanDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Payment>> GetByLoanIdAsync(Guid loanId)
    {
        return await _dbSet
            .Where(p => p.LoanId == loanId)
            .OrderByDescending(p => p.PaymentDate)
            .ToListAsync();
    }

    public async Task<decimal> GetTotalPaidByLoanIdAsync(Guid loanId)
    {
        return await _dbSet
            .Where(p => p.LoanId == loanId)
            .SumAsync(p => p.Amount);
    }

    public async Task<Payment?> GetByTransactionReferenceAsync(string reference)
    {
        return await _dbSet
            .Include(p => p.Loan)
            .FirstOrDefaultAsync(p => p.TransactionReference == reference);
    }
}