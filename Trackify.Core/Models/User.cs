using System;
using System.Collections.Generic;

namespace Trackify.Core.Models
{
    public class User
    {
        public Guid Id { get; private set; }
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string DisplayName { get; set; } = null!;
        public string Role { get; set; } = "User";
        public DateTime CreatedAt { get; private set; }

        public ICollection<TrackedProduct> TrackedProducts { get; set; } = new List<TrackedProduct>();

        private User() { } // EF Core constructor

        public User(string email, string passwordHash, string displayName, string role = "User")
        {
            Id = Guid.NewGuid();
            Email = email;
            PasswordHash = passwordHash;
            DisplayName = displayName;
            Role = role;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
