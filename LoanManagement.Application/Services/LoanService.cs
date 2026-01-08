using AutoMapper;
using LoanManagement.Application.DTOs.Common;
using LoanManagement.Application.DTOs.Loan;
using LoanManagement.Application.Interfaces;
using LoanManagement.Domain.Entities;
using LoanManagement.Domain.Enums;
using LoanManagement.Domain.Interfaces;

namespace LoanManagement.Application.Services;

public class LoanService : ILoanService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public LoanService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<LoanDto?> GetByIdAsync(Guid id)
    {
        var loan = await _unitOfWork.Loans.GetWithPaymentsAsync(id);
        return loan == null ? null : _mapper.Map<LoanDto>(loan);
    }

    public async Task<(IEnumerable<Loan> Items, int TotalCount)> GetPagedAsync(
        int pageNumber,
        int pageSize,
        LoanStatus? status = null,
        Guid? customerId = null)
    {
        var query = _dbSet.Include(l => l.Customer).AsQueryable();

        if (status.HasValue)
        {
            query = query.Where(l => l.Status == status.Value);
        }

        if (customerId.HasValue)
        {
            query = query.Where(l => l.CustomerId == customerId.Value);
        }

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(l => l.ApplicationDate)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<IEnumerable<LoanDto>> GetByCustomerIdAsync(Guid customerId)
    {
        var loans = await _unitOfWork.Loans.GetByCustomerIdAsync(customerId);
        return _mapper.Map<IEnumerable<LoanDto>>(loans);
    }

    public async Task<LoanDto> CreateAsync(CreateLoanDto dto)
    {
        var customerExists = await _unitOfWork.Customers.ExistsAsync(dto.CustomerId);
        if (!customerExists)
        {
            throw new KeyNotFoundException($"Customer with ID {dto.CustomerId} not found");
        }

        var loan = _mapper.Map<Loan>(dto);
        loan.Id = Guid.NewGuid();
        loan.CalculateMonthlyPayment();

        await _unitOfWork.Loans.AddAsync(loan);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<LoanDto>(await _unitOfWork.Loans.GetByIdAsync(loan.Id));
    }

    public async Task<LoanDto> ApproveAsync(Guid id)
    {
        var loan = await _unitOfWork.Loans.GetByIdAsync(id);
        if (loan == null)
        {
            throw new KeyNotFoundException($"Loan with ID {id} not found");
        }

        loan.Approve();
        await _unitOfWork.Loans.UpdateAsync(loan);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<LoanDto>(await _unitOfWork.Loans.GetByIdAsync(id));
    }

    public async Task<LoanDto> RejectAsync(Guid id, RejectLoanDto dto)
    {
        var loan = await _unitOfWork.Loans.GetByIdAsync(id);
        if (loan == null)
        {
            throw new KeyNotFoundException($"Loan with ID {id} not found");
        }

        loan.Reject(dto.Reason);
        await _unitOfWork.Loans.UpdateAsync(loan);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<LoanDto>(await _unitOfWork.Loans.GetByIdAsync(id));
    }

    public async Task<LoanDto> ActivateAsync(Guid id)
    {
        var loan = await _unitOfWork.Loans.GetByIdAsync(id);
        if (loan == null)
        {
            throw new KeyNotFoundException($"Loan with ID {id} not found");
        }

        loan.Activate();
        await _unitOfWork.Loans.UpdateAsync(loan);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<LoanDto>(await _unitOfWork.Loans.GetByIdAsync(id));
    }
}