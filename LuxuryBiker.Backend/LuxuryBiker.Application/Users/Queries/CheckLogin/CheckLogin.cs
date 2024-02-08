using LuxuryBiker.Domain.Entities.Users;
using LuxuryBiker.Domain.Repositories.Users;

namespace LuxuryBiker.Application.Users.Queries.CheckLogin;

[Authorize]
public record CheckLoginQuery(
                string Username,
                string Password,
                bool RememberMe
                ) : IRequest<ResponseGeneric<UserAuthenticated>>;

internal sealed class CheckLoginQueryhandler : IRequestHandler<CheckLoginQuery, ResponseGeneric<UserAuthenticated>>
{
    private readonly IRepositoryUser _repositoryUser;
    private readonly IAuthenticationService _authenticationService;
    public readonly string CheckLoginUnsuccessfully = "usuario y/o contraseña incorrectos";

    public CheckLoginQueryhandler(IRepositoryUser repositoryUser, IAuthenticationService authenticationService)
    {
        _repositoryUser = repositoryUser ?? throw new ArgumentNullException(nameof(repositoryUser));
        _authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
    }

    public async Task<ResponseGeneric<UserAuthenticated>> Handle(CheckLoginQuery request, CancellationToken cancellationToken)
    {
        string passwordDB = await _repositoryUser.GetPasswordAsync(request.Username.Trim());
        if (_authenticationService.CheckPassword(passwordDB, request.Password))
        {
            return new ResponseGeneric<UserAuthenticated>
            {
                Error = true,
                Message = CheckLoginUnsuccessfully
            };
        }

        User user = await _repositoryUser.GetUserAsync(request.Username.Trim());
        string token = _authenticationService.Authenticate(user.Id, user.Username!, request.RememberMe);
        return new ResponseGeneric<UserAuthenticated>
        {
            Error = false,
            Result = new UserAuthenticated
            {
                Token = token,
            }
        };
    }
}
