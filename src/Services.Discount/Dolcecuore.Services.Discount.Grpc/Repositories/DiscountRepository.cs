using System.Threading.Tasks;
using Dapper;
using Dolcecuore.Services.Discount.Grpc.Entities;
using Dolcecuore.Services.Discount.Grpc.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Dolcecuore.Services.Discount.Grpc.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly IConfiguration _configuration;

        public DiscountRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<Coupon> GetDiscount(string productName)
        {
            await using var connection = new NpgsqlConnection
                (_configuration["Postgres:ConnectionString"]);

            var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>
                ("select * from Coupon where ProductName = @ProductName", new {Productname = productName});

            if (coupon is null)
                return new Coupon
                {
                    ProductName = "No Discount",
                    Amount = 0,
                    Description = "No Discount Desc"
                };
            
            return coupon;
        }

        public async Task<bool> CreateDiscount(Coupon coupon)
        {
            await using var connection = new NpgsqlConnection
                (_configuration["Postgres:ConnectionString"]);

            var affected = await connection.ExecuteAsync
                ("INSERT INTO Coupon (ProductName, Description, Amount) VALUES (@ProductName, @Discount, @Amount)",
                    new {coupon.ProductName, coupon.Description, coupon.Amount});

            return affected is not 0;
        }

        public async Task<bool> UpdateDiscount(Coupon coupon)
        {
            await using var connection = new NpgsqlConnection
                (_configuration["Postgres:ConnectionString"]);

            var affected = await connection.ExecuteAsync
            ("UPDATE Coupon SET ProductName=@ProductName, Description=@Discount, Amount=@Amount WHERE Id=@Id",
                new {coupon.ProductName, coupon.Description, coupon.Amount, coupon.Id});

            return affected is not 0;
        }

        public async Task<bool> DeleteDiscount(string productName)
        {
            await using var connection = new NpgsqlConnection
                (_configuration["Postgres:ConnectionString"]);

            var affected = await connection.ExecuteAsync
            ("DELETE FROM Coupon WHERE ProductName=@ProductName",
                new {ProductName = productName});

            return affected is not 0;
        }
    }
}