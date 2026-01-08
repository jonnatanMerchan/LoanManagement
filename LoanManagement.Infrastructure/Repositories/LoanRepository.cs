using Microsoft.EntityFrameworkCore;
using LoanManagement.Domain.Entities;
using LoanManagement.Domain.Enums;
using LoanManagement.Domain.Interfaces;
using LoanManagement.Infrastructure.Data;

namespace LoanManagement.Infrastructure.Repositories;

public class LoanRepository : Repository<Loan>, ILoanRepository
{
    public LoanRepository(LoanDbContext context) : base(context)
    {
    }

    public override async Task<Loan?> GetByIdAsync(Guid id)
    {
        return await _dbSet
            .Include(l => l.Customer)
            .FirstOrDefaultAsync(l => l.Id == id);
    }

    public async Task<IEnumerable<Loan>> GetByCustomerIdAsync(Guid customerId)
    {
        return await _dbSet
            .Include(l => l.Customer)
            .Where(l => l.CustomerId == customerId)
            .OrderByDescending(l => l.ApplicationDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Loan>> GetByStatusAsync(LoanStatus status)
    {
        return await _dbSet
            .Include(l => l.Customer)
            .Where(l => l.Status == status)
            .OrderByDescending(l => l.ApplicationDate)
            .ToListAsync();
    }

public async Task<(IEnumerable<Loan> Items, int TotalCount)> GetPagedAsync(
    int pageNumber,
    int pageSize,
    LoanStatus? status = null,
    Guid? customerId = null)
{
    Console.WriteLine($"🔍 LoanRepository - Filtering by Status: {status}, CustomerId: {customerId}"); // Debug
    
    var query = _dbSet.Include(l => l.Customer).AsQueryable();

    if (status.HasValue)
    {
        Console.WriteLine($"✅ Applying status filter: {status.Value}"); // Debug
        query = query.Where(l => l.Status == status.Value);
    }

    if (customerId.HasValue)
    {
        Console.WriteLine($"✅ Applying customer filter: {customerId.Value}"); // Debug
        query = query.Where(l => l.CustomerId == customerId.Value);
    }

    var totalCount = await query.CountAsync();
    Console.WriteLine($"📊 Total count after filter: {totalCount}"); // Debug

    var items = await query
        .OrderByDescending(l => l.ApplicationDate)
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

    Console.WriteLine($"📦 Items returned: {items.Count}"); // Debug
    
    return (items, totalCount);
}

    public async Task<Loan?> GetWithPaymentsAsync(Guid id)
    {
        return await _dbSet
            .Include(l => l.Customer)
            .Include(l => l.Payments.OrderBy(p => p.PaymentDate))
            .FirstOrDefaultAsync(l => l.Id == id);
    }
}