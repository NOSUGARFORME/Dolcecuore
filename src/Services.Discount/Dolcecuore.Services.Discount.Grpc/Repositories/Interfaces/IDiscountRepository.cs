using System.Threading.Tasks;
using Dolcecuore.Services.Discount.Grpc.Entities;

namespace Dolcecuore.Services.Discount.Grpc.Repositories.Interfaces
{
    public interface IDiscountRepository
    {
        Task<Coupon> GetDiscount(string productName);

        Task<bool> CreateDiscount(Coupon coupon);
        Task<bool> UpdateDiscount(Coupon coupon);
        Task<bool> DeleteDiscount(string productName);
    }
}