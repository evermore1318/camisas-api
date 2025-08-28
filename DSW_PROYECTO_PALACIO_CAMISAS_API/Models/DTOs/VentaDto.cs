namespace DSW_PROYECTO_PALACIO_CAMISAS_API.Models.DTOs
{
    public class VentaDto
    {
        public int id_venta { get; set; }
        public string nombre_cliente { get; set; }
        public string dni_cliente { get; set; }
        public string tipo_pago { get; set; }
        public DateTime fecha { get; set; }
        public decimal precio_total { get; set; }
        public string estado { get; set; }
        public List<DetalleVentaDto> detalles { get; set; } = new List<DetalleVentaDto>();
    }

    public class DetalleVentaDto
    {
        public int id_camisa { get; set; }
        public string producto { get; set; }
        public string descripcion { get; set; }
        public string color { get; set; }
        public string talla { get; set; }
        public string manga { get; set; }
        public string marca { get; set; }
        public int cantidad { get; set; }
        public decimal precio { get; set; }
        public string estado { get; set; }
    }

    public class VentaUpdateDto
    {
        public string estado { get; set; }
    }

    public class VentaCreateDto
    {
        public string nombre_cliente { get; set; }
        public string dni_cliente { get; set; }
        public string tipo_pago { get; set; }
        public List<DetalleVentaCreateDto> detalles { get; set; }
    }
    public class DetalleVentaCreateDto
    {
        public int id_camisa { get; set; }
        public int cantidad { get; set; }
        public decimal precio { get; set; }
    }
   
}
