using Microsoft.AspNetCore.Mvc;
using MvcNetCoreSergioAlcazar.Models;
using MvcNetCoreSergioAlcazar.Repositories;

namespace TiendaExamen.ViewComponents
{
    public class MenuGenerosViewComponent: ViewComponent
    {
        RepositoryLibros repo;
        public MenuGenerosViewComponent(RepositoryLibros repo)
        {
            this.repo = repo;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Genero> generos = await this.repo.GetGeneros();
            return View(generos);
        }
    }
}
