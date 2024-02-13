using LuxuryBiker.Application.Common.Interfaces;
using LuxuryBiker.Infrastructure.Services.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace LuxuryBiker.Infrastructure.Persistence
{
    public class LuxuryBikerDbContext : IdentityDbContext<ApplicationUser>, ILuxuryBikerDbContext
    {
        public LuxuryBikerDbContext(DbContextOptions<LuxuryBikerDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }
    }
}
