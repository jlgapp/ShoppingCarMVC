using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShoppingCar.AccesoDatos.Datos.Repositorio.IRepositorio;
using ShoppingCar.Modelos.ViewModels;
using ShoppingCar.Utilidades;

namespace ShoppingCar.Controllers
{
	public class VentaController : Controller
	{
		private readonly IVentaRepositorio _ventaRepositorio;
		private readonly IVentaDetalleRepositorio _ventaDetalleRepositorio;

		public VentaController(IVentaRepositorio ventaRepositorio, IVentaDetalleRepositorio ventaDetalleRepositorio)
		{
			_ventaRepositorio = ventaRepositorio;
			_ventaDetalleRepositorio = ventaDetalleRepositorio;
		}

		public IActionResult Index()
		{
			VentaVM ventaVM = new VentaVM(){
				VentaLista = _ventaRepositorio.ObtenerTodos(),
				EstadoLista = WC.ListaEstados.ToList().Select(t=> new SelectListItem
				{
					Text = t,
					Value = t
				})
			};
			return View(ventaVM);
		}
	}
}
