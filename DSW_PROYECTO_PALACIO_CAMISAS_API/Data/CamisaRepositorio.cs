using DSW_PROYECTO_PALACIO_CAMISAS_API.Data.Contrato;
using DSW_PROYECTO_PALACIO_CAMISAS_API.Models;

namespace DSW_PROYECTO_PALACIO_CAMISAS_API.Data
{
    public class CamisaRepositorio : ICamisa
    {
        // Datos en memoria TEMPORALES
        private List<Camisa> _camisas = new List<Camisa>
        {
            new Camisa { id_camisa = 1, descripcion = "Camisa Deportiva Nike", color = "Azul", talla = "M", manga = "Corta", stock = 50, precio_costo = 15, precio_venta = 29.99m, estado = "Activo" },
            new Camisa { id_camisa = 2, descripcion = "Camisa Casual Adidas", color = "Blanca", talla = "L", manga = "Larga", stock = 30, precio_costo = 12, precio_venta = 24.99m, estado = "Activo" },
            new Camisa { id_camisa = 3, descripcion = "Camisa Elegante Puma", color = "Negra", talla = "S", manga = "Larga", stock = 20, precio_costo = 18, precio_venta = 34.99m, estado = "Activo" }
        };

        public List<Camisa> Listado()
        {
            return _camisas;
        }

        public Camisa ObtenerPorID(int id)
        {
            return _camisas.FirstOrDefault(c => c.id_camisa == id);
        }

        public Camisa Registrar(Camisa camisa)
        {
            camisa.id_camisa = _camisas.Max(c => c.id_camisa) + 1;
            _camisas.Add(camisa);
            return camisa;
        }

        public Camisa Actualizar(Camisa camisa)
        {
            var existente = _camisas.FirstOrDefault(c => c.id_camisa == camisa.id_camisa);
            if (existente != null)
            {
                existente.descripcion = camisa.descripcion;
                existente.color = camisa.color;
                existente.talla = camisa.talla;
                existente.manga = camisa.manga;
                existente.stock = camisa.stock;
                existente.precio_venta = camisa.precio_venta;
                existente.estado = camisa.estado;
            }
            return existente;
        }

        public bool Eliminar(int id)
        {
            var camisa = _camisas.FirstOrDefault(c => c.id_camisa == id);
            if (camisa != null)
            {
                _camisas.Remove(camisa);
                return true;
            }
            return false;
        }
    }
}