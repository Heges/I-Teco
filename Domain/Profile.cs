using Domain.Interfaces;
using System;

namespace Domain
{
    public class Profile : IIdentity
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; }
        public Guid ContactId { get; set; }
        public Contact Contact { get; set; }
    }
}
