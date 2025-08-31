namespace DSW_PROYECTO_PALACIO_CAMISAS_API.Models.DTOs
{
    public class UsuarioRequest
    {
        public string Nombre { get; set; }
        public string Password { get; set; }
        public int IdRol { get; set; }
        public string Estado { get; set; }
    }
}
