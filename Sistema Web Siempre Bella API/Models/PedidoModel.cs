namespace Sistema_Web_Siempre_Bella_API.Models
{
    public class PedidoModel
    {
        public int NoPedido { get; set; }
        public DateTime? FechaEntrega { get; set; }
        public int IDEmpleado { get; set; }
        public decimal TotalPedido { get; set; }
        public int IDProveedores { get; set; }
    }
}
