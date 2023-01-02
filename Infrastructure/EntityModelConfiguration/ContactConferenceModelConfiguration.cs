using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.EntityModelConfiguration
{
    public class ContactConferenceModelConfiguration : IEntityTypeConfiguration<ContactConference>
    {
        public void Configure(EntityTypeBuilder<ContactConference> builder)
        {
            builder.HasKey(entity => new { entity.ContactId, entity.ConferenceId});
        }
    }
}
