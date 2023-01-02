using Domain;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application
{
    public interface IApplicationDbContext
    {
        DbSet<Contact> Contacts { get; set; }
        DbSet<Phone> Phones { get; set; }
        DbSet<Conference> Conferences { get; set; }
        DbSet<Profile> Profiles { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancelationToken);
    }
}
