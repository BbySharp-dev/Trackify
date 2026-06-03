using System;
using System.Collections.Generic;

namespace Trackify.Core.Models
{
    public class Product
    {
        public Guid Id { get; private set; }
        public string Name { get; set; } = null!;
        public string Url { get; set; } = null!;
        public string Source { get; set; } = null!;
        public decimal? CurrentPrice { get; private set; }
        public decimal? LowestPrice { get; private set; }
        public decimal? HighestPrice { get; private set; }
        public string? ImageUrl { get; set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        public ICollection<PriceHistory> PriceHistories { get; set; } = new List<PriceHistory>();

        private Product() { } // EF Core constructor

        public Product(string name, string url, string source, string? imageUrl = null)
        {
            Id = Guid.NewGuid();
            Name = name;
            Url = url;
            Source = source;
            ImageUrl = imageUrl;
            CreatedAt = DateTime.UtcNow;
        }

        public void UpdatePrice(decimal newPrice)
        {
            CurrentPrice = newPrice;
            LowestPrice = LowestPrice == null ? newPrice : Math.Min(LowestPrice.Value, newPrice);
            HighestPrice = HighestPrice == null ? newPrice : Math.Max(HighestPrice.Value, newPrice);
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
