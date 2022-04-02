using ShoppingCar.Modelos;

namespace ShoppingCar.AccesoDatos.Datos.Repositorio.IRepositorio
{
    public interface IOrdenDetalleRepositorio : IRepositorio<OrdenDetalle>
    {
        void Actualizar(OrdenDetalle entidad);        
    }
}
