using Microsoft.AspNetCore.Mvc;
using LoanManagement.Application.DTOs.Common;
using LoanManagement.Application.DTOs.Loan;
using LoanManagement.Application.Interfaces;
using LoanManagement.Domain.Enums;

namespace LoanManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LoansController : ControllerBase
{
    private readonly ILoanService _loanService;
    private readonly ILogger<LoansController> _logger;

    public LoansController(ILoanService loanService, ILogger<LoansController> logger)
    {
        _loanService = loanService;
        _logger = logger;
    }

    /// <summary>
    /// Get all loans with pagination and optional filters
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<LoanDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<LoanDto>>>> GetLoans(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] LoanStatus? status = null,
        [FromQuery] Guid? customerId = null)
    {
        _logger.LogInformation("Getting loans - Page: {PageNumber}, Status: {Status}, CustomerId: {CustomerId}", 
            pageNumber, status, customerId);
        
        var result = await _loanService.GetPagedAsync(pageNumber, pageSize, status, customerId);
        return Ok(ApiResponse<PagedResult<LoanDto>>.SuccessResponse(result));
    }

    /// <summary>
    /// Get loan by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<LoanDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<LoanDto>>> GetLoan(Guid id)
    {
        _logger.LogInformation("Getting loan with ID: {LoanId}", id);
        
        var loan = await _loanService.GetByIdAsync(id);
        if (loan == null)
        {
            return NotFound(ApiResponse<LoanDto>.ErrorResponse($"Loan with ID {id} not found"));
        }

        return Ok(ApiResponse<LoanDto>.SuccessResponse(loan));
    }

    /// <summary>
    /// Get all loans for a specific customer
    /// </summary>
    [HttpGet("customer/{customerId}")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<LoanDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IEnumerable<LoanDto>>>> GetLoansByCustomer(Guid customerId)
    {
        _logger.LogInformation("Getting loans for customer: {CustomerId}", customerId);
        
        var loans = await _loanService.GetByCustomerIdAsync(customerId);
        return Ok(ApiResponse<IEnumerable<LoanDto>>.SuccessResponse(loans));
    }

    /// <summary>
    /// Create a new loan application
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<LoanDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<LoanDto>>> CreateLoan([FromBody] CreateLoanDto dto)
    {
        _logger.LogInformation("Creating loan for customer: {CustomerId}, Amount: {Amount}", 
            dto.CustomerId, dto.Amount);
        
        var loan = await _loanService.CreateAsync(dto);
        return CreatedAtAction(
            nameof(GetLoan),
            new { id = loan.Id },
            ApiResponse<LoanDto>.SuccessResponse(loan, "Loan application created successfully"));
    }

    /// <summary>
    /// Approve a loan
    /// </summary>
    [HttpPut("{id}/approve")]
    [ProducesResponseType(typeof(ApiResponse<LoanDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<LoanDto>>> ApproveLoan(Guid id)
    {
        _logger.LogInformation("Approving loan: {LoanId}", id);
        
        var loan = await _loanService.ApproveAsync(id);
        return Ok(ApiResponse<LoanDto>.SuccessResponse(loan, "Loan approved successfully"));
    }

    /// <summary>
    /// Reject a loan
    /// </summary>
    [HttpPut("{id}/reject")]
    [ProducesResponseType(typeof(ApiResponse<LoanDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<LoanDto>>> RejectLoan(Guid id, [FromBody] RejectLoanDto dto)
    {
        _logger.LogInformation("Rejecting loan: {LoanId}, Reason: {Reason}", id, dto.Reason);
        
        var loan = await _loanService.RejectAsync(id, dto);
        return Ok(ApiResponse<LoanDto>.SuccessResponse(loan, "Loan rejected successfully"));
    }

    /// <summary>
    /// Activate an approved loan
    /// </summary>
    [HttpPut("{id}/activate")]
    [ProducesResponseType(typeof(ApiResponse<LoanDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<LoanDto>>> ActivateLoan(Guid id)
    {
        _logger.LogInformation("Activating loan: {LoanId}", id);
        
        var loan = await _loanService.ActivateAsync(id);
        return Ok(ApiResponse<LoanDto>.SuccessResponse(loan, "Loan activated successfully"));
    }
}