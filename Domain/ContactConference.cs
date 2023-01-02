using System;

namespace Domain
{
    public class ContactConference
    {
        public Guid ContactId { get; set; }
        public Contact Contact { get; set; }
        public Guid ConferenceId { get; set; }
        public Conference Conference { get; set; }
    }
}
