using System.ComponentModel.DataAnnotations;

namespace ShoppingCar.Models
{
    public class TipoAplicacion
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Nombre tipo aplicacion es obligatorio")]
        public string Nombre { get; set; }
    }
}
