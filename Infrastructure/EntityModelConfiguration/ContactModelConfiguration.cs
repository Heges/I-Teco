using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityModelConfiguration
{
    public class ContactModelConfiguration : IEntityTypeConfiguration<Contact>
    {
        /// <summary>
        ///  EF может сопоставить данные верно с соблюдением nameconvences
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<Contact> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(p => p.Profile).WithOne(p => p.Contact).HasForeignKey<Profile>(p => p.ContactId);
            builder.HasOne(p => p.Phone).WithOne(p => p.Contact).HasForeignKey<Phone>(p => p.ContactId);
            builder.HasMany(p => p.CallingHistories).WithOne(x => x.LogOwner).HasForeignKey(x => x.CallerId);
        }
    }
}
