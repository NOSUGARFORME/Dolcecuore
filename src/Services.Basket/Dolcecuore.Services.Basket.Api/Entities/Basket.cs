using System.Collections.Generic;

namespace Dolcecuore.Services.Basket.Api.Entities
{
    public class Basket
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
