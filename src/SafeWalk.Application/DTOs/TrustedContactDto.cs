using System;
using System.Collections.Generic;
using System.Text;

namespace SafeWalk.Application.DTOs
{
    public class TrustedContactDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string ContactName { get; set; } = default!;
        public string ContactPhoneNumber { get; set; } = default!;
        public string? Relationship { get; set; }
    }

}
