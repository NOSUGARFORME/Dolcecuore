namespace Dolcecuore.Services.Basket.Entities
{
    public class BasketItem
    {
        public string ProductName { get; set; }
        public string ProductId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}