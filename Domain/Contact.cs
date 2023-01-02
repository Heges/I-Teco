using Domain.Interfaces;
using System;
using System.Collections.Generic;

namespace Domain
{
    public class Contact : IIdentity
    {
        public Guid Id { get; set; }
        public Profile Profile { get; set; }
        public Phone Phone { get; set; }
        public virtual ICollection<ContactConference> Conferences { get; set; }
        public virtual ICollection<CallingHistory> CallingHistories { get; set; }
    }
}
