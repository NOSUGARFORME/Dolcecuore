using Dolcecuore.Services.Discount.Grpc.Protos;

namespace Dolcecuore.Services.Basket.Services;

public interface IDiscountGrpcService
{
    Task<CouponModel> GetDiscount(string productName);
}