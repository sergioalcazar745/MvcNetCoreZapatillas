using Microsoft.AspNetCore.Mvc;
using MvcNetCoreZapatillas.Models;
using MvcNetCoreZapatillas.Repositories;

namespace MvcNetCoreZapatillas.Controllers
{
    public class ZapatillasController : Controller
    {
        RepositoryZapatillas repo;
        public ZapatillasController(RepositoryZapatillas repo)
        {
            this.repo = repo;
        }

        public async Task<IActionResult> Zapatillas()
        {
            List<Zapatilla> zapatillas = await this.repo.GetZapatillas();
            return View(zapatillas);
        }

        public async Task<IActionResult> DetallesZapatilla(int? idposicion, int idproducto)
        {
            if(idposicion == null)
            {
                idposicion = 1;
            }
            ViewData["POSICION"] = idposicion;
            Zapatilla zapatilla = await this.repo.GetZapatilla(idproducto);
            return View(zapatilla);
        }

        public async Task<IActionResult> _ImagenesZapatilla(int idposicion, int idproducto)
        {
            ModelPaginarZapatilla modelZapatilla = await this.repo.GetImagenZapatilla(idposicion, idproducto); 
            return PartialView("_ImagenesZapatilla", modelZapatilla);
        }

        //public async Task<IActionResult> Imagenes(int idposicion, int idproducto)
        //{
        //    ModelPaginarZapatilla modelZapatilla = await this.repo.GetImagenZapatilla(idposicion, idproducto);
        //    return View(modelZapatilla);
        //}
    }
}
