using AutoMapper;
using InvoiceManagement.API.DTOs;
using InvoiceManagement.API.Models;

namespace InvoiceManagement.API.Profiles
{
    public class InvoiceProfile : Profile
    {
        public InvoiceProfile()
        {
            CreateMap<Invoice, InvoiceReadDto>().ReverseMap();

            CreateMap<Invoice, InvoiceCreateDto>().ReverseMap()
                .ForMember(invoice => invoice.Id, options => options.Ignore())
                .ForMember(invoice => invoice.Number, options => options.Ignore())
                .ForMember(invoice => invoice.CreatedAt, options => options.Ignore());

            CreateMap<Invoice, InvoiceUpdateDto>().ReverseMap()
                .ForMember(invoice => invoice.Id, options => options.Ignore())
                .ForMember(invoice => invoice.Number, options => options.Ignore())
                .ForMember(invoice => invoice.ClientName, options => options.Ignore())
                .ForMember(invoice => invoice.CreatedAt, options => options.Ignore());
        }
    }
}
