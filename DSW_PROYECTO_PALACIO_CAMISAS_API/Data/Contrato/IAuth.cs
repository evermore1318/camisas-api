using DSW_PROYECTO_PALACIO_CAMISAS_API.Models.DTOs;

namespace DSW_PROYECTO_PALACIO_CAMISAS_API.Data.Contrato
{
    public interface IAuth
    {
        UsuarioDTO Login(string usuario, string clave);
    }
}
