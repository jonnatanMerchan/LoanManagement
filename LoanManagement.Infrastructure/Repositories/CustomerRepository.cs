using Microsoft.EntityFrameworkCore;
using LoanManagement.Domain.Entities;
using LoanManagement.Domain.Interfaces;
using LoanManagement.Infrastructure.Data;

namespace LoanManagement.Infrastructure.Repositories;

public class CustomerRepository : Repository<Customer>, ICustomerRepository
{
    public CustomerRepository(LoanDbContext context) : base(context)
    {
    }

    public async Task<Customer?> GetByEmailAsync(string email)
    {
        return await _dbSet
            .FirstOrDefaultAsync(c => c.Email.ToLower() == email.ToLower());
    }

    public async Task<IEnumerable<Customer>> SearchAsync(string searchTerm)
    {
        var lowerTerm = searchTerm.ToLower();
        return await _dbSet
            .Where(c => c.FirstName.ToLower().Contains(lowerTerm) ||
                       c.LastName.ToLower().Contains(lowerTerm) ||
                       c.Email.ToLower().Contains(lowerTerm))
            .OrderBy(c => c.LastName)
            .ThenBy(c => c.FirstName)
            .ToListAsync();
    }

    public async Task<(IEnumerable<Customer> Items, int TotalCount)> GetPagedAsync(
        int pageNumber, 
        int pageSize)
    {
        var query = _dbSet.AsQueryable();
        var totalCount = await query.CountAsync();
        
        var items = await query
            .OrderBy(c => c.LastName)
            .ThenBy(c => c.FirstName)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<bool> EmailExistsAsync(string email, Guid? excludeId = null)
    {
        var query = _dbSet.Where(c => c.Email.ToLower() == email.ToLower());
        
        if (excludeId.HasValue)
        {
            query = query.Where(c => c.Id != excludeId.Value);
        }

        return await query.AnyAsync();
    }
}