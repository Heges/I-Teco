using Domain.Interfaces;
using System;

namespace Domain
{
    public class CallingHistory : IIdentity
    {
        public Guid Id { get; set; }
        public Guid CallerId { get; set; }
        public Contact LogOwner { get; set; }
        public string CallerName { get; set; }
        public string CallerPhone { get; set; }
        public string CalledName { get; set; }
        public string CalledPhone { get; set; }
        public DateTime DateCall { get; set; }
    }
}
