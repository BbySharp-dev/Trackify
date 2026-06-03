using System;

namespace Trackify.Core.Models
{
    public class TrackedProduct
    {
        public Guid Id { get; private set; }
        public Guid UserId { get; private set; }
        public Guid ProductId { get; private set; }
        public decimal? TargetPrice { get; set; }
        public bool IsNotificationEnabled { get; set; } = true;
        public DateTime CreatedAt { get; private set; }

        public User User { get; set; } = null!;
        public Product Product { get; set; } = null!;

        private TrackedProduct() { } // EF Core constructor

        public TrackedProduct(Guid userId, Guid productId, decimal? targetPrice = null)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            ProductId = productId;
            TargetPrice = targetPrice;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
