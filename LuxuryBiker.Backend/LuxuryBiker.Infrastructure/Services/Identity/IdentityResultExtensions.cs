using Microsoft.AspNetCore.Identity;

namespace LuxuryBiker.Infrastructure.Services.Identity
{
    public static class IdentityResultExtensions
    {
        public static Application.Common.Models.Result ToApplicationResult(this IdentityResult result)
        {
            return result.Succeeded
                ? Application.Common.Models.Result.Success()
                : Application.Common.Models.Result.Failure(result.Errors.Select(e => e.Description));
        }
    }
}