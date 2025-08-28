using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace DSW_PROYECTO_PALACIO_CAMISAS_WebApp.Models
{
    public class Pedido
    {
        public int IdPedido { get; set; }
        public string Descripcion { get; set; }
        public int IdProveedor { get; set; }

        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }
        public decimal Monto { get; set; }
        public string? Estado { get; set; }

        public int CantidadPagos { get; set; }
        public decimal MontoTotalPagos { get; set; }
        public decimal DeudaPendiente { get; set; }

        // Relación con proveedor
        public Proveedor? Proveedor { get; set; }

        // Relación con pagos (un pedido puede tener varios pagos)
        public ICollection<Pago>? Pagos { get; set; }
    }
}
