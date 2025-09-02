using LuxuryBiker.Application.Common.Interfaces;
using LuxuryBiker.Domain.Entities.Common;
using LuxuryBiker.Domain.Entities.Products;
using LuxuryBiker.Domain.Entities.Purchases;
using LuxuryBiker.Domain.Entities.Sales;
using LuxuryBiker.Domain.Entities.Thirds;
using LuxuryBiker.Domain.Entities.Users;
using LuxuryBiker.Infrastructure.Persistence.Metadata.Products;
using LuxuryBiker.Infrastructure.Persistence.Metadata.Purchases;
using LuxuryBiker.Infrastructure.Persistence.Metadata.Sales;
using LuxuryBiker.Infrastructure.Persistence.Metadata.Thirds;
using MediatR;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace LuxuryBiker.Infrastructure.Persistence
{
    public class LuxuryBikerDbContext : IdentityDbContext<ApplicationUser>, ILuxuryBikerDbContext
    {
        #region Purchases
        public DbSet<Purchase> Purchases { get; set; } = null!;
        public DbSet<PurchaseDetail> PurchaseDetails { get; set; } = null!;
        #endregion
        #region Products
        public DbSet<Product> Products { get; set; } = null!;
        #endregion
        #region Thirds
        public DbSet<Third> Thirds { get; set; } = null!;
        public DbSet<TypeThird> TypeThird { get; set; } = null!;
        #endregion
        #region Sales
        public DbSet<Sale> Sales { get; set; } = null!;
        public DbSet<SaleDetail> SaleDetails { get; set; } = null!;
        #endregion

        private readonly ILogger<LuxuryBikerDbContext> _logger;
        private readonly IPublisher _publisher;

        public LuxuryBikerDbContext(DbContextOptions<LuxuryBikerDbContext> options, IPublisher publisher,
            ILogger<LuxuryBikerDbContext> logger) : base(options) 
        {
            _publisher = publisher;
            _logger = logger;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            #region Thirds
            ThirdsMetadata.SetEntityBuilder(builder.Entity<Third>());
            ThirdTypeMetadata.SetEntityBuilder(builder.Entity<TypeThird>());
            #endregion
            #region purchases
            PurchasesMetadata.SetEntityBuilder(builder.Entity<Purchase>());
            PurchaseDetailsMetadata.SetEntityBuilder(builder.Entity<PurchaseDetail>());
            #endregion
            #region sales
            SalesMetadata.SetEntityBuilder(builder.Entity<Sale>());
            SaleDetailsMetadata.SetEntityBuilder(builder.Entity<SaleDetail>());
            #endregion
            #region Products
            ProductsMetadata.SetEntityBuilder(builder.Entity<Product>());
            #endregion

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var result = await base.SaveChangesAsync(cancellationToken);

            var events = ChangeTracker.Entries<BaseEntity<int>>()
                    .Select(x => x.Entity.DomainEvents)
                    .SelectMany(x => x)
                    .Where(domainEvent => !domainEvent.IsPublished)
                    .ToArray().Union(ChangeTracker.Entries<BaseEntity<string>>()
                    .Select(x => x.Entity.DomainEvents)
                    .SelectMany(x => x)
                    .Where(domainEvent => !domainEvent.IsPublished)
                    .ToArray()).ToArray();

            foreach (var @event in events)
            {
                @event.IsPublished = true;

                _logger.LogInformation("New domain event {Event}", @event.GetType().Name);

                await _publisher.Publish(@event, cancellationToken);
            }

            return result;
        }
    }
}
