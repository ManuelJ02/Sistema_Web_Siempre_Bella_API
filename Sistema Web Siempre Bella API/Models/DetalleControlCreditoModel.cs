namespace Sistema_Web_Siempre_Bella_API.Models
{
    public class DetalleControlCreditoModel
    {
        public int IDDetalleCredito { get; set; }
        public decimal MontoCuota { get; set; }
        public int NumeroCuota { get; set; }
        public int IDTipoFactura { get; set; }
        public int NoFactura { get; set; }
    }
}
