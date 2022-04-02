using System.ComponentModel.DataAnnotations;

namespace ShoppingCar.Modelos
{
    public class Categoria
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage ="Nombre Categoria es obligatorio")]
        public string NombreCategoria { get; set; }
        [Required(ErrorMessage = "Orden Categoria es obligatorio")]
        [Range(1, int.MaxValue, ErrorMessage ="El orden debe ser mayor a 0")]
        public int MostrarOrden { get; set; }
    }
}
