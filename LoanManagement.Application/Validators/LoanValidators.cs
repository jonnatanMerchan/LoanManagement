using FluentValidation;
using LoanManagement.Application.DTOs.Loan;

namespace LoanManagement.Application.Validators;

public class CreateLoanValidator : AbstractValidator<CreateLoanDto>
{
    public CreateLoanValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("Customer ID is required");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than zero")
            .LessThanOrEqualTo(1000000).WithMessage("Amount cannot exceed 1,000,000");

        RuleFor(x => x.InterestRate)
            .GreaterThanOrEqualTo(0).WithMessage("Interest rate cannot be negative")
            .LessThanOrEqualTo(100).WithMessage("Interest rate cannot exceed 100%");

        RuleFor(x => x.TermInMonths)
            .GreaterThan(0).WithMessage("Term must be at least 1 month")
            .LessThanOrEqualTo(360).WithMessage("Term cannot exceed 360 months (30 years)");
    }
}

public class RejectLoanValidator : AbstractValidator<RejectLoanDto>
{
    public RejectLoanValidator()
    {
        RuleFor(x => x.Reason)
            .NotEmpty().WithMessage("Rejection reason is required")
            .MaximumLength(500).WithMessage("Rejection reason cannot exceed 500 characters");
    }
}