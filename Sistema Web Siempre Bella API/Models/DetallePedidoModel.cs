namespace Sistema_Web_Siempre_Bella_API.Models
{
    public class DetallePedidoModel
    {
        public int IDDetallePedido { get; set; }
        public int CantidadProductos { get; set; }
        public int IDProducto { get; set; }
        public decimal SubtotalPedido { get; set; }
        public int NoPedido { get; set; }
    }
}
