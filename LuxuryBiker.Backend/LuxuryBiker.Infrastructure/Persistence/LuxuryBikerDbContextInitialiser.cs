using LuxuryBiker.Domain.Constants;
using LuxuryBiker.Domain.Entities.Thirds;
using LuxuryBiker.Infrastructure.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LuxuryBiker.Infrastructure.Persistence
{
    public static class InitialiserExtensions
    {
        public static async Task InitialiseDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var initialiser = scope.ServiceProvider.GetRequiredService<LuxuryBikerDbContextInitialiser>();

            await initialiser.InitialiseAsync();

            await initialiser.SeedAsync();
        }
    }
    public class LuxuryBikerDbContextInitialiser
    {
        private readonly ILogger<LuxuryBikerDbContextInitialiser> _logger;
        private readonly LuxuryBikerDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public LuxuryBikerDbContextInitialiser(ILogger<LuxuryBikerDbContextInitialiser> logger, LuxuryBikerDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task InitialiseAsync()
        {
            try
            {
                await _context.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while initialising the database.");
                throw;
            }
        }

        public async Task SeedAsync()
        {
            try
            {
                await TrySeedAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while seeding the database.");
                throw;
            }
        }

        public async Task TrySeedAsync()
        {
            // Default roles
            var administratorRole = new IdentityRole(Roles.Administrator);

            if (_roleManager.Roles.All(r => r.Name != administratorRole.Name))
            {
                await _roleManager.CreateAsync(administratorRole);
            }

            // Default users
            var administrator = new ApplicationUser { 
                UserName = "administrator@luxurybiker.com", 
                Email = "administrator@luxurybiker.com",
                Identification = "123456789",
                Names = "Christian",
                Active = true,
                Surnames = "Garzón",
            };


            if (_userManager.Users.All(u => u.UserName != administrator.UserName))
            {
                await _userManager.CreateAsync(administrator, "Administrator1!");
                if (!string.IsNullOrWhiteSpace(administratorRole.Name))
                {
                    await _userManager.AddToRolesAsync(administrator, new[] { administratorRole.Name });
                }
            }

            // Tipos de tercero: los ids son parte del contrato (ThirdTypes.Provider/Client),
            // así que se insertan explícitamente y no dependen del orden de inserción.
            await EnsureThirdTypeAsync(ThirdTypes.Provider, "Provider");
            await EnsureThirdTypeAsync(ThirdTypes.Client, "Client");
        }

        /// <summary>
        /// Inserta el tipo de tercero con un id fijo si no existe. Es idempotente por tipo
        /// (antes, si faltaba solo uno de los dos, no se creaba ninguno) y evita que el id
        /// dependa del orden en que se siembran las filas.
        /// </summary>
        private async Task EnsureThirdTypeAsync(int id, string name)
        {
            if (await _context.TypeThird.AnyAsync(x => x.Id == id))
            {
                return;
            }

            if (await _context.TypeThird.AnyAsync(x => x.Name == name))
            {
                _logger.LogWarning(
                    "El tipo de tercero '{Name}' existe con un id distinto del esperado ({ExpectedId}). " +
                    "Los combos de compras y ventas usan ese id, revise los datos.", name, id);
                return;
            }

            // La PK es IDENTITY: para fijar el id hay que habilitar IDENTITY_INSERT.
            await _context.Database.ExecuteSqlRawAsync(
                "SET IDENTITY_INSERT T_TYPE_THIRD ON; " +
                "INSERT INTO T_TYPE_THIRD (Id, Name, Active) VALUES ({0}, {1}, 1); " +
                "SET IDENTITY_INSERT T_TYPE_THIRD OFF;", id, name);
        }
    }
}
