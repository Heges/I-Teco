using Microsoft.Extensions.DependencyInjection;
using System;

namespace Infrastructure
{
    public class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();
        }
    }
}
