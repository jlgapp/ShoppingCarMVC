using ShoppingCar.Modelos;

namespace ShoppingCar.AccesoDatos.Datos.Repositorio.IRepositorio
{
    public interface IVentaRepositorio : IRepositorio<Venta>
    {
        void Actualizar(Venta entidad);        
    }
}
