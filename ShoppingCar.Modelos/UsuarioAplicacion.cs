using Microsoft.AspNetCore.Identity;

namespace ShoppingCar.Modelos
{
    public class UsuarioAplicacion : IdentityUser
    {
        public string NombreCompleto { get; set; }
    }
}
