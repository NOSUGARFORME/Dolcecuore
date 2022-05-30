using AutoMapper;

namespace Dolcecuore.Services.Order.Api.Models;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Entities.Order, OrderModel>().ReverseMap();
    }
}