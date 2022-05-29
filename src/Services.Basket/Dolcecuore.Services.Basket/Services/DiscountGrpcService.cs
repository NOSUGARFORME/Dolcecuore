using Dolcecuore.Infrastructure.Grpc;
using Dolcecuore.Services.Discount.Grpc.Protos;
using Microsoft.Extensions.Configuration;

namespace Dolcecuore.Services.Basket.Services
{
    public class DiscountGrpcService : IDiscountGrpcService
    {
        private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoService;

        public DiscountGrpcService(IConfiguration configuration)
        {
            _discountProtoService = new DiscountProtoService.DiscountProtoServiceClient(ChannelFactory.Create(configuration["Services:Grpc:Discount"]));
        }

        public async Task<CouponModel> GetDiscount(string productName)
        {
            var discountRequest = new GetDiscountRequest {ProductName = productName};
            return await _discountProtoService.GetDiscountAsync(discountRequest);
        }
    }
}