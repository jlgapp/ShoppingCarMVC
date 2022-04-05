using ShoppingCar.Modelos;

namespace ShoppingCar.AccesoDatos.Datos.Repositorio.IRepositorio
{
    public interface IVentaDetalleRepositorio : IRepositorio<VentaDetalle>
    {
        void Actualizar(VentaDetalle entidad);        
    }
}
