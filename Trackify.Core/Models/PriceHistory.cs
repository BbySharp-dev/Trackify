using System;

namespace Trackify.Core.Models
{
    public class PriceHistory
    {
        public Guid Id { get; private set; }
        public Guid ProductId { get; private set; }
        public decimal Price { get; private set; }
        public string Currency { get; private set; } = "VND";
        public DateTime ScrapedAt { get; private set; }

        public Product Product { get; set; } = null!;

        private PriceHistory() { } // EF Core constructor

        public PriceHistory(Guid productId, decimal price, string currency = "VND")
        {
            Id = Guid.NewGuid();
            ProductId = productId;
            Price = price;
            Currency = currency;
            ScrapedAt = DateTime.UtcNow;
        }
    }
}
