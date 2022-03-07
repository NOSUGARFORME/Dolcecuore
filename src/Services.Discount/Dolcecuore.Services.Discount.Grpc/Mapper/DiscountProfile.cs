using AutoMapper;
using Dolcecuore.Services.Discount.Grpc.Entities;
using Dolcecuore.Services.Discount.Grpc.Protos;

namespace Dolcecuore.Services.Discount.Grpc.Mapper
{
    public class DiscountProfile : Profile
    {
        public DiscountProfile()
        {
            CreateMap<Coupon, CouponModel>().ReverseMap();
        }
    }
}