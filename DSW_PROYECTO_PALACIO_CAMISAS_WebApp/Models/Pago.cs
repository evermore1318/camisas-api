namespace DSW_PROYECTO_PALACIO_CAMISAS_WebApp.Models
{
    public class Pago
    {
        public int IdPago { get; set; }
        public int IdPedido { get; set; }
        public string Descripcion { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Monto { get; set; }
        public string? Estado { get; set; }

        // Relación con pedido
        public Pedido? Pedido { get; set; }
    }
}
