namespace DSW_PROYECTO_PALACIO_CAMISAS_API.Models
{
    public class Venta
    {
        public int id_venta { get; set; }
        public string nombre_cliente { get; set; }
        public string dni_cliente { get; set; }
        public string tipo_pago { get; set; }
        public DateTime fecha { get; set; }
        public decimal precio_total { get; set; }
        public string estado { get; set; }

        public List<DetalleVenta>? detalles { get; set; }
    }
}
