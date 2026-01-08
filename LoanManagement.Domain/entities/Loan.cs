using LoanManagement.Domain.Enums;

namespace LoanManagement.Domain.Entities;

public class Loan : BaseEntity
{
    public Guid CustomerId { get; set; }
    public decimal Amount { get; set; }
    public decimal InterestRate { get; set; }
    public int TermInMonths { get; set; }
    public decimal MonthlyPayment { get; set; }
    public LoanStatus Status { get; set; }
    public DateTime ApplicationDate { get; set; }
    public DateTime? ApprovalDate { get; set; }
    public string? RejectionReason { get; set; }

    // Navigation properties
    public Customer Customer { get; set; } = null!;
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();

    // Business methods
    public void CalculateMonthlyPayment()
    {
        if (TermInMonths <= 0 || Amount <= 0)
        {
            MonthlyPayment = 0;
            return;
        }

        var monthlyRate = InterestRate / 100 / 12;
        
        if (monthlyRate == 0)
        {
            MonthlyPayment = Amount / TermInMonths;
        }
        else
        {
            var numerator = Amount * monthlyRate * (decimal)Math.Pow((double)(1 + monthlyRate), TermInMonths);
            var denominator = (decimal)Math.Pow((double)(1 + monthlyRate), TermInMonths) - 1;
            MonthlyPayment = Math.Round(numerator / denominator, 2);
        }
    }

    public decimal GetTotalAmount() => MonthlyPayment * TermInMonths;
    public decimal GetTotalInterest() => GetTotalAmount() - Amount;
    
    public decimal GetRemainingBalance()
    {
        var totalPaid = Payments.Sum(p => p.Amount);
        return GetTotalAmount() - totalPaid;
    }

    public void Approve()
    {
        if (Status != LoanStatus.Pending)
            throw new InvalidOperationException("Only pending loans can be approved");
        Status = LoanStatus.Approved;
        ApprovalDate = DateTime.UtcNow;
    }

    public void Reject(string reason)
    {
        if (Status != LoanStatus.Pending)
            throw new InvalidOperationException("Only pending loans can be rejected");
        Status = LoanStatus.Rejected;
        RejectionReason = reason;
    }

    public void Activate()
    {
        if (Status != LoanStatus.Approved)
            throw new InvalidOperationException("Only approved loans can be activated");
        Status = LoanStatus.Active;
    }

    public void Complete()
    {
        if (GetRemainingBalance() > 0.01m)
            throw new InvalidOperationException("Loan still has remaining balance");
        Status = LoanStatus.Completed;
    }
}