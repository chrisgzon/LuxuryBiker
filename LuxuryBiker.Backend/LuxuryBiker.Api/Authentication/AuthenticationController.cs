using LuxuryBiker.Api.Common;
using LuxuryBiker.Application.Common.Interfaces.Services;
using LuxuryBiker.Application.WeatherForecasts.Queries.GetWeatherForecasts;
using LuxuryBiker.Infrastructure.Services.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace LuxuryBiker.Api.Authentication
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AuthenticationController : ApiController
    {
        private readonly IAuthenticationService<ApplicationUserDTO> _authenticationService;
        public AuthenticationController(IAuthenticationService<ApplicationUserDTO> authenticationService)
        {
            _authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
        }

        [HttpPost]
        public async Task<IActionResult> Login(AuthenticationModel requestModel)
        {
            ErrorOr<string> response = await _authenticationService.Authenticate(requestModel.Username, requestModel.Password, requestModel.Rememberme);
            return response.Match(
                value => Ok(JsonSerializer.Serialize(value)),
                errors => Problem(errors)
            );
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetProfileCurrentUser()
        {
            ErrorOr<ApplicationUserDTO> response = await _authenticationService.GetCurrentUserProfile();
            return response.Match(
                value => Ok(value),
                errors => Problem(errors)
            );
        }
    }
}