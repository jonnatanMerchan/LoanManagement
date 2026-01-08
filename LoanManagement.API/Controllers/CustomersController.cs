using Microsoft.AspNetCore.Mvc;
using LoanManagement.Application.DTOs.Common;
using LoanManagement.Application.DTOs.Customer;
using LoanManagement.Application.Interfaces;

namespace LoanManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;
    private readonly ILogger<CustomersController> _logger;

    public CustomersController(ICustomerService customerService, ILogger<CustomersController> logger)
    {
        _customerService = customerService;
        _logger = logger;
    }

    /// <summary>
    /// Get all customers with pagination
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<CustomerDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<CustomerDto>>>> GetCustomers(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        _logger.LogInformation("Getting customers - Page: {PageNumber}, Size: {PageSize}", pageNumber, pageSize);
        
        var result = await _customerService.GetPagedAsync(pageNumber, pageSize);
        return Ok(ApiResponse<PagedResult<CustomerDto>>.SuccessResponse(result));
    }

    /// <summary>
    /// Get customer by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<CustomerDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<CustomerDto>>> GetCustomer(Guid id)
    {
        _logger.LogInformation("Getting customer with ID: {CustomerId}", id);
        
        var customer = await _customerService.GetByIdAsync(id);
        if (customer == null)
        {
            return NotFound(ApiResponse<CustomerDto>.ErrorResponse($"Customer with ID {id} not found"));
        }

        return Ok(ApiResponse<CustomerDto>.SuccessResponse(customer));
    }

    /// <summary>
    /// Search customers by name or email
    /// </summary>
    [HttpGet("search")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<CustomerDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IEnumerable<CustomerDto>>>> SearchCustomers([FromQuery] string q)
    {
        _logger.LogInformation("Searching customers with term: {SearchTerm}", q);
        
        var customers = await _customerService.SearchAsync(q);
        return Ok(ApiResponse<IEnumerable<CustomerDto>>.SuccessResponse(customers));
    }

    /// <summary>
    /// Create a new customer
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<CustomerDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<CustomerDto>>> CreateCustomer([FromBody] CreateCustomerDto dto)
    {
        _logger.LogInformation("Creating new customer with email: {Email}", dto.Email);
        
        var customer = await _customerService.CreateAsync(dto);
        return CreatedAtAction(
            nameof(GetCustomer),
            new { id = customer.Id },
            ApiResponse<CustomerDto>.SuccessResponse(customer, "Customer created successfully"));
    }

    /// <summary>
    /// Update an existing customer
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponse<CustomerDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<CustomerDto>>> UpdateCustomer(Guid id, [FromBody] UpdateCustomerDto dto)
    {
        _logger.LogInformation("Updating customer with ID: {CustomerId}", id);
        
        var customer = await _customerService.UpdateAsync(id, dto);
        return Ok(ApiResponse<CustomerDto>.SuccessResponse(customer, "Customer updated successfully"));
    }

    /// <summary>
    /// Delete a customer (soft delete)
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCustomer(Guid id)
    {
        _logger.LogInformation("Deleting customer with ID: {CustomerId}", id);
        
        await _customerService.DeleteAsync(id);
        return NoContent();
    }
}