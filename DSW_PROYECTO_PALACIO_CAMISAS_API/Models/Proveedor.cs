namespace DSW_PROYECTO_PALACIO_CAMISAS_API.Models
{
    public class Proveedor
    {
        public int IdProveedor { get; set; }
        public int IdMarca { get; set; }
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public string Email { get; set; }
        public string Estado { get; set; }

        // Relación con marca
        public Marca? Marca { get; set; }

        // Relación con pedidos (un proveedor puede tener varios pedidos)
        public ICollection<Pedido>? Pedidos { get; set; }
    }
}
