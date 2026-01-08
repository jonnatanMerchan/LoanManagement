using LoanManagement.Application.DTOs.Common;
using LoanManagement.Application.DTOs.Loan;
using LoanManagement.Domain.Enums;

namespace LoanManagement.Application.Interfaces;

public interface ILoanService
{
    Task<LoanDto?> GetByIdAsync(Guid id);
    Task<PagedResult<LoanDto>> GetPagedAsync(int pageNumber, int pageSize, LoanStatus? status = null, Guid? customerId = null);
    Task<IEnumerable<LoanDto>> GetByCustomerIdAsync(Guid customerId);
    Task<LoanDto> CreateAsync(CreateLoanDto dto);
    Task<LoanDto> ApproveAsync(Guid id);
    Task<LoanDto> RejectAsync(Guid id, RejectLoanDto dto);
    Task<LoanDto> ActivateAsync(Guid id);
}