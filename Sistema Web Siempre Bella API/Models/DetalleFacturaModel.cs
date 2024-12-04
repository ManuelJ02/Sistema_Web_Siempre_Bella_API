namespace Sistema_Web_Siempre_Bella_API.Models
{
    public class DetalleFacturaModel
    {
        public int IDDetalleFactura { get; set; }
        public int CantProductos { get; set; }
        public decimal SubTotalFactura { get; set; }
        public float IVA { get; set; }
        public int IDProducto { get; set; }
        public int NoFactura { get; set; }
    }
}
