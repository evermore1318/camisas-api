namespace DSW_PROYECTO_PALACIO_CAMISAS_WebApp.Models.DTOs
{
    public class VentaCreateDto
    {
        public string nombre_cliente { get; set; } = "";
        public string dni_cliente { get; set; } = "";
        public string tipo_pago { get; set; } = "";
        public List<DetalleVentaCreateDto> detalles { get; set; } = new();
    }
}
