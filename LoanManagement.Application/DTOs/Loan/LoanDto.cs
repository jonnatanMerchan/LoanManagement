using LoanManagement.Domain.Enums;

namespace LoanManagement.Application.DTOs.Loan;

public class LoanDto
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public decimal InterestRate { get; set; }
    public int TermInMonths { get; set; }
    public decimal MonthlyPayment { get; set; }
    public LoanStatus Status { get; set; }
    public string StatusName { get; set; } = string.Empty;
    public DateTime ApplicationDate { get; set; }
    public DateTime? ApprovalDate { get; set; }
    public string? RejectionReason { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal TotalInterest { get; set; }
    public decimal RemainingBalance { get; set; }
    public int PaymentsMade { get; set; }
}

public class CreateLoanDto
{
    public Guid CustomerId { get; set; }
    public decimal Amount { get; set; }
    public decimal InterestRate { get; set; }
    public int TermInMonths { get; set; }
}

public class ApproveLoanDto
{
    public string? Notes { get; set; }
}

public class RejectLoanDto
{
    public string Reason { get; set; } = string.Empty;
}