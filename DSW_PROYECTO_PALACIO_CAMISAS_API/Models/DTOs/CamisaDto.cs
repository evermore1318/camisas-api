namespace DSW_PROYECTO_PALACIO_CAMISAS_API.Models.DTOs
{
    public class CamisaDto
    {
        public int id_camisa { get; set; }
        public string descripcion { get; set; }
        public string marca { get; set; }
        public string color { get; set; }
        public string talla { get; set; }
        public string manga { get; set; }
        public int stock { get; set; }
        public decimal precio_costo { get; set; }
        public decimal precio_venta { get; set; }
        public string estante { get; set; }
        public string estado { get; set; }
        public int id_marca { get; set; }
        public int id_estante { get; set; }
    }
}
