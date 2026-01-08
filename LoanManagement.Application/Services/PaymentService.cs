using AutoMapper;
using LoanManagement.Application.DTOs.Payment;
using LoanManagement.Application.Interfaces;
using LoanManagement.Domain.Entities;
using LoanManagement.Domain.Enums;
using LoanManagement.Domain.Interfaces;

namespace LoanManagement.Application.Services;

public class PaymentService : IPaymentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public PaymentService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PaymentDto?> GetByIdAsync(Guid id)
    {
        var payment = await _unitOfWork.Payments.GetByIdAsync(id);
        return payment == null ? null : _mapper.Map<PaymentDto>(payment);
    }

    public async Task<IEnumerable<PaymentDto>> GetByLoanIdAsync(Guid loanId)
    {
        var payments = await _unitOfWork.Payments.GetByLoanIdAsync(loanId);
        return _mapper.Map<IEnumerable<PaymentDto>>(payments);
    }
    public async Task<PaymentDto> CreateAsync(CreatePaymentDto dto)
    {
        var loan = await _unitOfWork.Loans.GetWithPaymentsAsync(dto.LoanId);
        if (loan == null)
        {
            throw new KeyNotFoundException($"Loan with ID {dto.LoanId} not found");
        }

        if (loan.Status != LoanStatus.Active && loan.Status != LoanStatus.Approved)
        {
            throw new InvalidOperationException($"Cannot make payment for loan in status {loan.Status}");
        }

        var existingPayment = await _unitOfWork.Payments.GetByTransactionReferenceAsync(dto.TransactionReference);
        if (existingPayment != null)
        {
            throw new InvalidOperationException($"Transaction reference '{dto.TransactionReference}' already exists");
        }

        var remainingBalance = loan.GetRemainingBalance();
        if (dto.Amount > remainingBalance)
        {
            throw new InvalidOperationException($"Payment amount ({dto.Amount:C}) exceeds remaining balance ({remainingBalance:C})");
        }

        try
        {
            var payment = _mapper.Map<Payment>(dto);
            payment.Id = Guid.NewGuid();

            await _unitOfWork.Payments.AddAsync(payment);
            
            if (loan.Status == LoanStatus.Approved)
            {
                loan.Activate();
                await _unitOfWork.Loans.UpdateAsync(loan);
            }

            await _unitOfWork.SaveChangesAsync();
            loan = await _unitOfWork.Loans.GetWithPaymentsAsync(dto.LoanId);
            
            if (loan!.GetRemainingBalance() <= 0.01m)
            {
                loan.Complete();
                await _unitOfWork.Loans.UpdateAsync(loan);
            }

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<PaymentDto>(payment);
        }
        catch
        {
            throw;
        }
    }

    public async Task<decimal> GetTotalPaidByLoanIdAsync(Guid loanId)
    {
        return await _unitOfWork.Payments.GetTotalPaidByLoanIdAsync(loanId);
    }
}