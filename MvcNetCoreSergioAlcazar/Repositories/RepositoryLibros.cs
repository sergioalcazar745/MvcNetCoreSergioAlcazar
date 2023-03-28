using Microsoft.EntityFrameworkCore;
using MvcNetCoreSergioAlcazar.Data;
using MvcNetCoreSergioAlcazar.Models;

namespace MvcNetCoreSergioAlcazar.Repositories
{
    public class RepositoryLibros
    {
        LibrosContext context;
        public RepositoryLibros(LibrosContext context)
        {
            this.context = context;
        }

        public async Task<List<Libro>> GetLibros()
        {
            return await this.context.Libros.ToListAsync();
        }

        public async Task<List<Libro>> GetLibrosGenero(int idgenero)
        {
            var consulta = from datos in this.context.Libros
                           where datos.IdGenero == idgenero
                           select datos;

            return await consulta.ToListAsync();
        }

        public async Task<Libro> GetLibro(int id)
        {
            return await this.context.Libros.FirstOrDefaultAsync(z => z.IdLibro == id);
        }

        public async Task<List<Genero>> GetGeneros()
        {
            return await this.context.Generos.ToListAsync();
        }

        public List<Libro> GetLibros10(int posicion, ref int numero)
        {
            List<Libro> productos = this.context.Libros.ToList();
            numero = productos.Count;
            List<Libro> filter = productos.Skip(posicion).Take(10).ToList();
            return filter;
        }

        public async Task<Usuario> GetUsuario(int id)
        {
            return await this.context.Usuarios.FirstOrDefaultAsync(z => z.IdUsuario == id);
        }

        public async Task<Usuario> ExisteUsuario(string username, string password)
        {
            return await this.context.Usuarios.FirstOrDefaultAsync(z => z.Email == username && z.Pass == password);
        }

        public async Task<int> GetMaxPedido()
        {
            if(this.context.Pedidos.Count() == 0)
            {
                return 1;
            }
            else
            {
                return this.context.Pedidos.Max(z => z.IdPedido) + 1;
            }
        }

        public async Task InsertPedido(List<Libro> libros, int idusuario)
        {
            foreach (Libro libro in libros)
            {
                Pedido pedido = new Pedido();
                pedido.IdPedido = await this.GetMaxPedido();
                pedido.IdFactura = 1;
                pedido.Fecha = DateTime.Now;
                pedido.IdLibro = libro.IdLibro;
                pedido.IdUsuario = idusuario;
                pedido.Cantidad = 1;
                await this.context.Pedidos.AddAsync(pedido);
            }
            await this.context.SaveChangesAsync();
        }

        public async Task <List<VistaPedido>> VistaPedidos(int idusuario)
        {
            var consulta = from datos in this.context.VistaPedidos
                           where datos.idUsuario == idusuario
                           select datos;

            return await consulta.ToListAsync();
        }
    }
}
