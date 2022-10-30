using AutoMapper;
using DomainModels = Core.Domain.Models;
using Entities = Core.DataAccess.Entities;

namespace Infrastructure.Domain.Mappers;

public class CustomerMapper : Profile
{
    public CustomerMapper()

    {
        CreateMap<DomainModels.Customer, Entities.Customer>()
            .ForMember(dest => dest.Id, a => a.MapFrom(src => src.CustomerId))
            .ForMember(dest => dest.Id, a => a.AllowNull());
        CreateMap<Entities.Customer, DomainModels.Customer>()
            .ForMember(dest => dest.CustomerId, a => a.MapFrom(src => src.Id));
    }
}
