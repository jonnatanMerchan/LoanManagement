using LoanManagement.Application.DTOs.Payment;

namespace LoanManagement.Application.Interfaces;

public interface IPaymentService
{
    Task<PaymentDto?> GetByIdAsync(Guid id);
    Task<IEnumerable<PaymentDto>> GetByLoanIdAsync(Guid loanId);
    Task<PaymentDto> CreateAsync(CreatePaymentDto dto);
    Task<decimal> GetTotalPaidByLoanIdAsync(Guid loanId);
}