using ErrorOr;

namespace LuxuryBiker.Domain.Entities.Thirds
{
    public class ThirdsErrors
    {
        public static Error Exists { get; } = Error.Validation(
            code: "Thirds.AlreadyExists",
            description: "El Tercero con la identificación ingresada ya se encuentra registrado en el sistema."
        );
    }
}
