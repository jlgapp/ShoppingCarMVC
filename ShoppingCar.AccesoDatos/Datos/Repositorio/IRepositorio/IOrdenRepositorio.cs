using ShoppingCar.Modelos;

namespace ShoppingCar.AccesoDatos.Datos.Repositorio.IRepositorio
{
    public interface IOrdenRepositorio : IRepositorio<Orden>
    {
        void Actualizar(Orden entidad);        
    }
}
