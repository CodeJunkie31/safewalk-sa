using SafeWalk.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace SafeWalk.Domain.Entities
{
    public class TrustedContact : BaseEntity
    {
        public Guid UserId { get; set; }
        public string ContactName { get; set; } = default!;
        public string ContactPhoneNumber { get; set; } = default!;
        public string? Relationship { get; set; }

        public User User { get; set; } = default!;
    }
}
