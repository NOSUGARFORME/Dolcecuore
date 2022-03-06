using System.Threading.Tasks;
using Dolcecuore.Services.Discount.Api.Entities;

namespace Dolcecuore.Services.Discount.Api.Repositories.Interfaces
{
    public interface IDiscountRepository
    {
        Task<Coupon> GetDiscount(string productName);

        Task<bool> CreateDiscount(Coupon coupon);
        Task<bool> UpdateDiscount(Coupon coupon);
        Task<bool> DeleteDiscount(string productName);
    }
}