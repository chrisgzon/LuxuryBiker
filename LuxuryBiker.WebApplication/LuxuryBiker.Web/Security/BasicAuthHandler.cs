using LuxuryBiker.Data.CustomTypes.Users;
using LuxuryBiker.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace LuxuryBiker.web.Security
{
    public class BasicAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private ILuxuryBikerService _luxuryBikerService;

        public BasicAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, 
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            ILuxuryBikerService service
            ) : base(options, logger, encoder, clock)
        {
            _luxuryBikerService = service;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return AuthenticateResult.Fail("Parametro no recibido");

            bool result = false;
            var user = new Users();
            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
                var credentials = Encoding.UTF8.GetString(credentialBytes).Split(new[] { ':' }, 2);
                var email = credentials[0];
                var password = credentials[1];
                user = _luxuryBikerService.Login(email, password).Value;
                result = user != null;
            }
            catch (Exception ex)
            {

                return AuthenticateResult.Fail("Ocurrio un error");
            }

            if (!result) return AuthenticateResult.Fail("Credenciales Invalidas");

            var claims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserName)
            };

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            return AuthenticateResult.Success(ticket);
        }
    }
}
