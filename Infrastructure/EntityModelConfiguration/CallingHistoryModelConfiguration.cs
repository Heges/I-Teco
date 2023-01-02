using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityModelConfiguration
{
    public class CallingHistoryModelConfiguration : IEntityTypeConfiguration<CallingHistory>
    {
        public void Configure(EntityTypeBuilder<CallingHistory> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }
}
