﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingCar.Modelos
{
    public class UsuarioAplicacion : IdentityUser
    {
        public string NombreCompleto { get; set; }
        [NotMapped]
        public string Direccion { get; set; }
        [NotMapped]
        public string Ciudad { get; set; }

    }
}
