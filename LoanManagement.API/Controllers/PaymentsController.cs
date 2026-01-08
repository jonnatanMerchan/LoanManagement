using Microsoft.AspNetCore.Mvc;
using LoanManagement.Application.DTOs.Common;
using LoanManagement.Application.DTOs.Payment;
using LoanManagement.Application.Interfaces;

namespace LoanManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentService _paymentService;
    private readonly ILogger<PaymentsController> _logger;

    public PaymentsController(IPaymentService paymentService, ILogger<PaymentsController> logger)
    {
        _paymentService = paymentService;
        _logger = logger;
    }

    /// <summary>
    /// Get payment by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<PaymentDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<PaymentDto>>> GetPayment(Guid id)
    {
        _logger.LogInformation("Getting payment with ID: {PaymentId}", id);
        
        var payment = await _paymentService.GetByIdAsync(id);
        if (payment == null)
        {
            return NotFound(ApiResponse<PaymentDto>.ErrorResponse($"Payment with ID {id} not found"));
        }

        return Ok(ApiResponse<PaymentDto>.SuccessResponse(payment));
    }

    /// <summary>
    /// Get all payments for a specific loan
    /// </summary>
    [HttpGet("loan/{loanId}")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<PaymentDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IEnumerable<PaymentDto>>>> GetPaymentsByLoan(Guid loanId)
    {
        _logger.LogInformation("Getting payments for loan: {LoanId}", loanId);
        
        var payments = await _paymentService.GetByLoanIdAsync(loanId);
        return Ok(ApiResponse<IEnumerable<PaymentDto>>.SuccessResponse(payments));
    }

    /// <summary>
    /// Create a new payment
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<PaymentDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<PaymentDto>>> CreatePayment([FromBody] CreatePaymentDto dto)
    {
        _logger.LogInformation("Creating payment for loan: {LoanId}, Amount: {Amount}", 
            dto.LoanId, dto.Amount);
        
        var payment = await _paymentService.CreateAsync(dto);
        return CreatedAtAction(
            nameof(GetPayment),
            new { id = payment.Id },
            ApiResponse<PaymentDto>.SuccessResponse(payment, "Payment registered successfully"));
    }

    /// <summary>
    /// Get total amount paid for a loan
    /// </summary>
    [HttpGet("loan/{loanId}/total")]
    [ProducesResponseType(typeof(ApiResponse<decimal>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<decimal>>> GetTotalPaid(Guid loanId)
    {
        _logger.LogInformation("Getting total paid for loan: {LoanId}", loanId);
        
        var total = await _paymentService.GetTotalPaidByLoanIdAsync(loanId);
        return Ok(ApiResponse<decimal>.SuccessResponse(total));
    }
}