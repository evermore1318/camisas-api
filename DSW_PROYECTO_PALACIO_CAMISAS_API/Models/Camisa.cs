using System.Text.Json.Serialization;

namespace DSW_PROYECTO_PALACIO_CAMISAS_API.Models
{
    public class Camisa
    {
        public int id_camisa { get; set; }
        public string descripcion { get; set; }
        public int id_marca { get; set; }
        public string color { get; set; }
        public string talla { get; set; }
        public string manga { get; set; }
        public int stock { get; set; }
        public decimal precio_costo { get; set; }
        public decimal precio_venta { get; set; }
        public int id_estante { get; set; }
        public string estado { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? marca_nombre { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? estante_descripcion { get; set; }
    }
}
