using Microsoft.AspNetCore.Mvc;
using MvcNetCoreSergioAlcazar.Extensions;
using MvcNetCoreSergioAlcazar.Models;
using MvcNetCoreSergioAlcazar.Repositories;
using System.Diagnostics;
using System.Security.Claims;
using TiendaExamen.Filters;

namespace MvcNetCoreSergioAlcazar.Controllers
{
    public class LibrosController : Controller
    {
        private RepositoryLibros repo;
        public LibrosController(RepositoryLibros repo)
        {
            this.repo = repo;
        }

        public IActionResult Libros(int? posicion)
        {
            if(posicion == null)
            {
                posicion = 0;
            }

            int numero = 0;
            List<Libro> libros = this.repo.GetLibros10(posicion.Value, ref numero);

            int siguiente = posicion.Value + 10;
            if (siguiente >= numero)
            {
                siguiente = 0;
            }
            int anterior = posicion.Value - 10;
            if (anterior < 0)
            {
                anterior = numero - 10;
            }
            ViewData["SIGUIENTE"] = siguiente;
            ViewData["ANTERIOR"] = anterior;

            return View(libros);
        }

        public async Task<IActionResult> LibrosGenero(int idgenero)
        {
            List<Libro> libros = await this.repo.GetLibrosGenero(idgenero);
            return View(libros);
        }

        public async Task<IActionResult> DetallesLibros(int id)
        {
            Libro libro = await this.repo.GetLibro(id);
            return View(libro);
        }

        public async Task<IActionResult> AñadirCarrito(int id)
        {
            List<Libro> libros = null;
            if (HttpContext.Session.GetObject<List<Libro>>("Carrito") == null)
            {
                libros = new List<Libro>();
            }
            else
            {
                libros = HttpContext.Session.GetObject<List<Libro>>("Carrito");
            }

            Libro libro = await this.repo.GetLibro(id);
            libros.Add(libro);
            HttpContext.Session.SetObject("Carrito", libros);

            return RedirectToAction("DetallesLibros", new { id = id });
        }


        public async Task<IActionResult> EliminarCarrito(int id)
        {
            List<Libro> libros = libros = HttpContext.Session.GetObject<List<Libro>>("Carrito");
            Libro libro = libros.Find(z => z.IdLibro == id);
            libros.Remove(libro);
            HttpContext.Session.SetObject("Carrito", libros);

            return RedirectToAction("Carrito");
        }

        public async Task<IActionResult> Carrito()
        {
            List<Libro> lista = HttpContext.Session.GetObject<List<Libro>>("Carrito");
            return View(lista);
        }

        [AuthorizeUsers]
        public async Task<IActionResult> FinalizarCompra()
        {
            int idusuario = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            List<Libro> libros = HttpContext.Session.GetObject <List<Libro>>("Carrito");
            if(libros == null)
            {
                return RedirectToAction("Libros", "Libros");
            }
            await this.repo.InsertPedido(libros, idusuario);
            List<VistaPedido> pedidos = await this.repo.VistaPedidos(idusuario);
            return View(pedidos);
        }
    }
}