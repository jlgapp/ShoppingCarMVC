using Microsoft.AspNetCore.Mvc.Rendering;

namespace ShoppingCar.Modelos.ViewModels
{
	public class VentaVM
	{
		public IEnumerable<Venta> VentaLista { get; set; }
		public IEnumerable<SelectListItem> EstadoLista { get; set; }
		public string Estado { get; set; }
	}
}
