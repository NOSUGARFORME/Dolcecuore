using Dolcecuore.Domain.Entities;

namespace Dolcecuore.Services.Basket.Entities
{
    public class Basket : AggregateRoot<Guid>
    {
        public string UserName { get; set; }
        public List<BasketItem> Items { get; set; } = new();

        public Basket()
        {
        }
        
        public Basket(string userName)
        {
            UserName = userName;
        }
        
        public decimal Total
        {
            get
            {
                decimal total = 0;
                foreach (var item in Items)
                {
                    total = item.Quantity * item.Price;
                }
        
                return total;
            }
        }
    }
}
