using AutoMapper;
using LoanManagement.Domain.Entities;
using LoanManagement.Domain.Enums;
using LoanManagement.Application.DTOs.Customer;
using LoanManagement.Application.DTOs.Loan;
using LoanManagement.Application.DTOs.Payment;

namespace LoanManagement.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Customer mappings
        CreateMap<Customer, CustomerDto>()
            .ForMember(dest => dest.TotalLoans, opt => opt.MapFrom(src => src.Loans.Count))
            .ForMember(dest => dest.ActiveLoans, opt => opt.MapFrom(src => 
                src.Loans.Count(l => l.Status == LoanStatus.Active)));

        CreateMap<CreateCustomerDto, Customer>();
        CreateMap<UpdateCustomerDto, Customer>();

        // Loan mappings
        CreateMap<Loan, LoanDto>()
            .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.FullName))
            .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.GetTotalAmount()))
            .ForMember(dest => dest.TotalInterest, opt => opt.MapFrom(src => src.GetTotalInterest()))
            .ForMember(dest => dest.RemainingBalance, opt => opt.MapFrom(src => src.GetRemainingBalance()))
            .ForMember(dest => dest.PaymentsMade, opt => opt.MapFrom(src => src.Payments.Count));

        CreateMap<CreateLoanDto, Loan>()
            .ForMember(dest => dest.ApplicationDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => LoanStatus.Pending));

        // Payment mappings
        CreateMap<Payment, PaymentDto>();
        CreateMap<CreatePaymentDto, Payment>()
            .ForMember(dest => dest.PaymentDate, opt => opt.MapFrom(src => DateTime.UtcNow));
    }
}