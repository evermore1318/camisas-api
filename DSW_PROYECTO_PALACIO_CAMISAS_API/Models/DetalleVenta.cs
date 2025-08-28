using System.Text.Json.Serialization;

namespace DSW_PROYECTO_PALACIO_CAMISAS_API.Models
{
    public class DetalleVenta
    {
        public int id_venta { get; set; }
        public int id_camisa { get; set; }
        public int cantidad { get; set; }
        public decimal precio { get; set; }
        public string estado { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? camisa_descripcion { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? camisa_color { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? camisa_talla { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? camisa_manga { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? marca_nombre { get; set; }
    }
}
