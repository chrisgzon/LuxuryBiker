namespace LuxuryBiker.Domain.Common
{
    /// <summary>
    /// Porción de un listado junto con el total de elementos disponibles.
    /// Evita que los puertos de repositorio devuelvan tuplas anónimas.
    /// </summary>
    public record Page<T>(IReadOnlyList<T> Items, int TotalCount)
    {
        public static Page<T> Empty { get; } = new(Array.Empty<T>(), 0);
    }
}
