namespace LoanManagement.Domain.Entities;

public class Customer : BaseEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public bool IsDeleted { get; set; }

    // Navigation property
    public ICollection<Loan> Loans { get; set; } = new List<Loan>();

    // Computed property
    public string FullName => $"{FirstName} {LastName}";
}