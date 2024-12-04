using System.Text.Json.Serialization;

namespace Sistema_Web_Siempre_Bella_API.Models
{
    public class ControlCreditoModel
    {
        public int IDTipoFactura { get; set; }
        public DateTime? FechaInicial { get; set; }
        public float? Interes { get; set; }
        public int Plazos { get; set; }

        // Propiedad de solo lectura que devuelve la fecha en el formato deseado
        [JsonIgnore]  // Ignorar esta propiedad si solo quieres formatear al retornar la respuesta
        public string FechaInicialFormatted => FechaInicial?.ToString("dd/MM/yyyy HH:mm");
    }
}
