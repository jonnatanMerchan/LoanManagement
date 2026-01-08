using AutoMapper;
using LoanManagement.Application.DTOs.Common;
using LoanManagement.Application.DTOs.Customer;
using LoanManagement.Application.Interfaces;
using LoanManagement.Domain.Entities;
using LoanManagement.Domain.Interfaces;

namespace LoanManagement.Application.Services;

public class CustomerService : ICustomerService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CustomerService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CustomerDto?> GetByIdAsync(Guid id)
    {
        var customer = await _unitOfWork.Customers.GetByIdAsync(id);
        return customer == null ? null : _mapper.Map<CustomerDto>(customer);
    }

    public async Task<PagedResult<CustomerDto>> GetPagedAsync(int pageNumber, int pageSize)
    {
        var (items, totalCount) = await _unitOfWork.Customers.GetPagedAsync(pageNumber, pageSize);
        
        return new PagedResult<CustomerDto>
        {
            Items = _mapper.Map<IEnumerable<CustomerDto>>(items),
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }

    public async Task<IEnumerable<CustomerDto>> SearchAsync(string searchTerm)
    {
        var customers = await _unitOfWork.Customers.SearchAsync(searchTerm);
        return _mapper.Map<IEnumerable<CustomerDto>>(customers);
    }

    public async Task<CustomerDto> CreateAsync(CreateCustomerDto dto)
    {
        if (await _unitOfWork.Customers.EmailExistsAsync(dto.Email))
        {
            throw new InvalidOperationException($"Email '{dto.Email}' is already registered");
        }

        var customer = _mapper.Map<Customer>(dto);
        customer.Id = Guid.NewGuid();

        await _unitOfWork.Customers.AddAsync(customer);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<CustomerDto>(customer);
    }

    public async Task<CustomerDto> UpdateAsync(Guid id, UpdateCustomerDto dto)
    {
        var customer = await _unitOfWork.Customers.GetByIdAsync(id);
        if (customer == null)
        {
            throw new KeyNotFoundException($"Customer with ID {id} not found");
        }

        if (customer.Email != dto.Email && await _unitOfWork.Customers.EmailExistsAsync(dto.Email, id))
        {
            throw new InvalidOperationException($"Email '{dto.Email}' is already registered");
        }

        _mapper.Map(dto, customer);
        await _unitOfWork.Customers.UpdateAsync(customer);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<CustomerDto>(customer);
    }

    public async Task DeleteAsync(Guid id)
    {
        var customer = await _unitOfWork.Customers.GetByIdAsync(id);
        if (customer == null)
        {
            throw new KeyNotFoundException($"Customer with ID {id} not found");
        }

        customer.IsDeleted = true;
        await _unitOfWork.Customers.UpdateAsync(customer);
        await _unitOfWork.SaveChangesAsync();
    }
}