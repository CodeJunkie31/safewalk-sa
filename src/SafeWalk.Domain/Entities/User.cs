using SafeWalk.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace SafeWalk.Domain.Entities
{
    public class User : BaseEntity
    {
        public string FullName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public string PasswordHash { get; set; } = default!;

        public ICollection<TrustedContact> TrustedContacts { get; set; } = new List<TrustedContact>();
        public ICollection<Journey> Journeys { get; set; } = new List<Journey>();
    }
}
