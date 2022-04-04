using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingCar.Modelos
{
    public class Producto
    {
		public Producto()
		{
            TempMetroCuadrado = 1;
		}

        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Nombre producto es requerido")]
        public string NombreProducto { get; set; }
        [Required(ErrorMessage = "Campos requerido")]
        public string DescripcionCorta { get; set; }
        [Required(ErrorMessage = "Campos requerido")]
        public string DescripcionProducto { get; set; }
        [Required(ErrorMessage = "Campos requerido")]
        [Range(1, double.MaxValue, ErrorMessage = "deme ser mayor  a cero")]
        public double Precio { get; set; }
        public string? ImagenUrl { get; set; }
        // fk
        public int CategoriaId { get; set; }
        [ForeignKey("CategoriaId")]
        public virtual Categoria? Categoria { get; set; }
        public int TipoAplicacionId { get; set; }
        [ForeignKey("TipoAplicacionId")]
        public virtual TipoAplicacion? TipoAplicacion { get; set; }

        [NotMapped]// campo para no agregar a la base de datos es tmp
        [Range(1, 10000)]
        public int TempMetroCuadrado { get; set; }

    }
}
