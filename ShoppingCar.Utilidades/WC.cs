using System.Collections.ObjectModel;

namespace ShoppingCar.Utilidades
{
    public static class WC
    {
        public static string ImagenRuta = @"\imagenes\producto\";
        public static string SessionCarroCompras = "SessionCarroCompras";
        public static string SessionOrdenId = "SessionOrden";

        public const string AdminRole = "Admin";
        public static string ClienteRole = "Cliente";
        public static string EmailAdmin = "garcia.arcalle@gmail.com";

        public const string CategoriaNombre = "Categoria";
        public const string TipoAplicacionNombre = "TipoAplicacion";

        public const string Exitosa = "Proceso Exitoso";
        public const string Error = "Proceso Con Error";

        public const string EstadoPendiente = "Pendiente";
        public const string EstadoAprobado = "Aprobado";
        public const string EstadoEnProceso = "Procesando";
        public const string EstadoEnviado = "Enviado";
        public const string EstadoCancelado = "Cancelado";
        public const string EstadoDevuelto = "Devuelto";

        public static readonly IEnumerable<string> ListaEstados = new ReadOnlyCollection<string>(
            new List<string>
            {
                EstadoPendiente,
                EstadoAprobado,
                EstadoEnProceso,
                EstadoEnviado ,
                EstadoCancelado,
                EstadoDevuelto
            });

    }
}
