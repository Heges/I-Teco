using Application;
using Domain;
using Infrastructure.EntityModelConfiguration;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Phone> Phones { get; set; }
        public DbSet<Conference> Conferences { get; set; }
        public DbSet<ContactConference> ContactConference { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<CallingHistory> CallingHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new ContactConferenceModelConfiguration());
            modelBuilder.ApplyConfiguration(new ConferenceModelConfiguration());
            modelBuilder.ApplyConfiguration(new PhoneModelConfiguration());
            modelBuilder.ApplyConfiguration(new ProfileModelConfiguration());
            modelBuilder.ApplyConfiguration(new ContactModelConfiguration());
            modelBuilder.ApplyConfiguration(new CallingHistoryModelConfiguration());

        }
    }
}
