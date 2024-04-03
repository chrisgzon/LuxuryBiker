using LuxuryBiker.Application.Common.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace LuxuryBiker.Api.Authentication;

public class CurrentUser : IUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? Id => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Sid);
}