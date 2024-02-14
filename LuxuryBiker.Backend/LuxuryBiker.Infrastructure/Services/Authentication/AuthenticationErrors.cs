namespace LuxuryBiker.Infrastructure.Services.Authentication
{
    public static class AuthenticationErrors
    {
        public static Error InvalidCredentials { get; } = Error.NotFound(
            code: "Authentication.InvalidCredentials",
            description: "usuario y/o contraseña invalidos."
        );
        public static Error NotUserlogged { get; } = Error.NotFound(
            code: "Authentication.NotUserLogged",
            description: "No se encontro informacion sobre loggeo de un usuario."
        );
    }
}