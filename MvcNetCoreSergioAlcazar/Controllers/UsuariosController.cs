using Microsoft.AspNetCore.Mvc;
using MvcNetCoreSergioAlcazar.Models;
using MvcNetCoreSergioAlcazar.Repositories;
using System.Security.Claims;
using TiendaExamen.Filters;

namespace MvcNetCoreSergioAlcazar.Controllers
{
    [AuthorizeUsers]
    public class UsuariosController : Controller
    {
        private RepositoryLibros repo;
        public UsuariosController(RepositoryLibros repo)
        {
            this.repo = repo;
        }

        public async Task<IActionResult> Perfil()
        {
            int id = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            Usuario usuario = await this.repo.GetUsuario(id);
            return View(usuario);
        }
    }
}
