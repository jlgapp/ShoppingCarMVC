using Microsoft.AspNetCore.Mvc.Rendering;
using ShoppingCar.Modelos;

namespace ShoppingCar.AccesoDatos.Datos.Repositorio.IRepositorio
{
    public interface IProductoRepositorio : IRepositorio<Producto>
    {
        void Actualizar(Producto producto);
        IEnumerable<SelectListItem> ObtenerTodosDropDownList(string obj);
    }
}
