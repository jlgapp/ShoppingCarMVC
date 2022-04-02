using ShoppingCar.Modelos;

namespace ShoppingCar.AccesoDatos.Datos.Repositorio.IRepositorio
{
    public interface IUsuarioAplicacionRepositorio : IRepositorio<UsuarioAplicacion>
    {
        void Actualizar(UsuarioAplicacion entidad);        
    }
}
