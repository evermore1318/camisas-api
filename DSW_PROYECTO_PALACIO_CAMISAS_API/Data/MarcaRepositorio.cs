using DSW_PROYECTO_PALACIO_CAMISAS_API.Data.Contrato;
using DSW_PROYECTO_PALACIO_CAMISAS_API.Models;

namespace DSW_PROYECTO_PALACIO_CAMISAS_API.Data
{
    public class MarcaRepositorio : IMarca
    {
        // Datos en memoria TEMPORALES para Marcas
        private List<Marca> _marcas = new List<Marca>
        {
            new Marca { id_marca = 1, descripcion = "Nike", estado = "Activo" },
            new Marca { id_marca = 2, descripcion = "Adidas", estado = "Activo" },
            new Marca { id_marca = 3, descripcion = "Puma", estado = "Activo" },
            new Marca { id_marca = 4, descripcion = "Under Armour", estado = "Activo" },
            new Marca { id_marca = 5, descripcion = "Reebok", estado = "Inactivo" }
        };

        public List<Marca> Listado()
        {
            return _marcas.Where(m => m.estado == "Activo").ToList();
        }
    }
}