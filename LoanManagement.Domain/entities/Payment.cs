namespace LoanManagement.Domain.Entities;

public class Payment : BaseEntity
{
    public Guid LoanId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public string TransactionReference { get; set; } = string.Empty;
    public string? Notes { get; set; }

    // Navigation property
    public Loan Loan { get; set; } = null!;
}