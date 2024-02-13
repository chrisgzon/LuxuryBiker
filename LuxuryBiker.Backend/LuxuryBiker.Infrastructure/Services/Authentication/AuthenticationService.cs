using AutoMapper;
using LuxuryBiker.Application.Common.Interfaces.Services;
using LuxuryBiker.Infrastructure.Services.Authentication.JWT;
using LuxuryBiker.Infrastructure.Services.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LuxuryBiker.Infrastructure.Services.Authentication
{
    public class AuthenticationService : IAuthenticationService<ApplicationUserDTO>
    {
        private readonly JWTSettings _jwtSettings;
        private readonly IIdentityService<ApplicationUser> _identityService;
        private readonly IUser _currentUser;
        private readonly IMapper _mapper;

        public AuthenticationService(
            IOptions<JWTSettings> jwtSettings,
            IIdentityService<ApplicationUser> identityService,
            IUser currentUser,
            IMapper mapper)
        {
            _jwtSettings = jwtSettings.Value;
            _identityService = identityService;
            _currentUser = currentUser;
            _mapper = mapper;
        }

        public async Task<ErrorOr<ApplicationUserDTO>> Authenticate(string username, string password, bool rememberMe)
        {
            // Verificamos credenciales con Identity
            ApplicationUser user = await _identityService.GetUserAsync(username);

            if (user is null || !await _identityService.ValidatePassword(user, password))
            {
                return AuthenticationErrors.InvalidCredentials;
            }

            ApplicationUserDTO userDTO = _mapper.Map<ApplicationUserDTO>(user);
            userDTO.Roles = await _identityService.GetRolesAsync(user);
            userDTO.Token = GenerateJWT(userDTO, rememberMe);
            return userDTO;
        }

        public async Task<ErrorOr<ApplicationUserDTO>> GetCurrentUserProfile()
        {
            if (string.IsNullOrEmpty(_currentUser.Id))
            {
                return AuthenticationErrors.NotUserlogged;
            }

            ApplicationUser user = await _identityService.GetUserByIdAsync(_currentUser.Id);
            ApplicationUserDTO userDTO = _mapper.Map<ApplicationUserDTO>(user);
            userDTO.Roles = await _identityService.GetRolesAsync(user);
            return userDTO;
        }

        private string GenerateJWT(
            ApplicationUserDTO user,
            bool rememberme)
        {
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            
            // Generamos un token según los claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Sid, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.GivenName, $"{user.Names} {user.Surnames}")
            };

            foreach (var role in user.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            JwtSecurityToken token = new JwtSecurityToken(
                _jwtSettings.Issuer,
                _jwtSettings.Audience,
                claims,
                expires: rememberme ? DateTime.UtcNow.AddMinutes(_jwtSettings.TokenExpirationInMinutes*2) : DateTime.UtcNow.AddMinutes(_jwtSettings.TokenExpirationInMinutes),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}