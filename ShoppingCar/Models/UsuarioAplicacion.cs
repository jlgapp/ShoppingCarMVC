using Microsoft.AspNetCore.Identity;

namespace ShoppingCar.Models
{
    public class UsuarioAplicacion : IdentityUser
    {
        public string NombreCompleto { get; set; }
    }
}
