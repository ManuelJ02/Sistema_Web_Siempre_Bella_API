namespace Sistema_Web_Siempre_Bella_API.Models
{
    public class ProductoModel
    {
        public int IDProducto { get; set; }
        public int Stock { get; set; }
        public decimal Precio { get; set; }
        public string Descripcion { get; set; }
        public int IDMarca { get; set; }
        public int IDCategoria { get; set; }
    }
}
