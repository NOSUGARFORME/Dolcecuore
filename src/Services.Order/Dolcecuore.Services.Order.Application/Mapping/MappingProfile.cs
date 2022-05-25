using AutoMapper;
using Dolcecuore.Services.Order.Application.Features.Orders.Commands.CheckoutOrder;
using Dolcecuore.Services.Order.Application.Features.Orders.Queries.GetOrderList;

namespace Dolcecuore.Services.Order.Application.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Domain.Entities.Order, OrderDto>().ReverseMap();
        CreateMap<Domain.Entities.Order, CheckoutOrderCommand>().ReverseMap();
    }
}