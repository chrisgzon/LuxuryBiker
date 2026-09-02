namespace LuxuryBiker.Application.Common.Models
{
    /// <summary>Resultado de invertir el estado de una compra o venta.</summary>
    public class ChangeStatusResult
    {
        public int Id { get; set; }
        public bool Status { get; set; }
    }
}
