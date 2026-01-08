using Microsoft.EntityFrameworkCore.Storage;
using LoanManagement.Domain.Interfaces;
using LoanManagement.Infrastructure.Data;

namespace LoanManagement.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly LoanDbContext _context;
    private IDbContextTransaction? _transaction;

    public ICustomerRepository Customers { get; }
    public ILoanRepository Loans { get; }
    public IPaymentRepository Payments { get; }

    public UnitOfWork(
        LoanDbContext context,
        ICustomerRepository customers,
        ILoanRepository loans,
        IPaymentRepository payments)
    {
        _context = context;
        Customers = customers;
        Loans = loans;
        Payments = payments;
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}