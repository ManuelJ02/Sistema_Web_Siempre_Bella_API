namespace Sistema_Web_Siempre_Bella_API.Models
{
    public class FacturaModel
    {
        public int NoFactura { get; set; }
        public decimal TotalFactura { get; set; }
        public DateTime? FechaFactura { get; set; }
        public int IDEmpleado { get; set; }
        public int IDCliente { get; set; }

    }
}
