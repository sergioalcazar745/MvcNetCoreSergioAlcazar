using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MvcNetCoreSergioAlcazar.Models;
using MvcNetCoreSergioAlcazar.Repositories;
using System.Security.Claims;

namespace MvcNetCoreSergioAlcazar.Controllers
{
    public class ManagedController : Controller
    {
        RepositoryLibros repo;
        public ManagedController(RepositoryLibros repo)
        {
            this.repo = repo;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            Usuario usuario = await this.repo.ExisteUsuario(username, password);

            if(usuario == null)
            {
                ViewData["Error"] = "Las credenciales son incorrectas";
                return View();
            }

            ClaimsIdentity identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme,
                                                        ClaimTypes.Name, ClaimTypes.Role);

            Claim claimName = new Claim(ClaimTypes.Name, usuario.Nombre);
            identity.AddClaim(claimName);
            Claim claimId = new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString());
            identity.AddClaim(claimId);
            ClaimsPrincipal userPrincipal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal);

            string controller = TempData["controller"].ToString();
            string action = TempData["action"].ToString();
            string id = TempData["id"].ToString();

            return RedirectToAction(action, controller
                , new { id = id });
        }

        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Remove("Carrito");
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Libros", "Libros");
        }


    }
}
