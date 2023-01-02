using System;
using System.Collections.Generic;
using Domain.Interfaces;

namespace Domain
{
    public class Conference : IIdentity
    {
        public Guid Id { get; set; }
        public DateTime DateConference { get; set; }
        public virtual ICollection<ContactConference> Contacts { get; set; }
    }
}
