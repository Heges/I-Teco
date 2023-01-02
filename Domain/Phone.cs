using Domain.Interfaces;
using System;

namespace Domain
{
    public class Phone : IIdentity
    {
        public Guid Id { get; set; }
        public string PhoneNumber { get; set; }
        public Guid ContactId { get; set; }
        public Contact Contact { get; set; }
    }
}
