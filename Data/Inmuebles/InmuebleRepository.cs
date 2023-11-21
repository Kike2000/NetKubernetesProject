using System.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NetKubernetes.Middleware;
using NetKubernetes.Models;
using NetKubernetes.Token;

namespace NetKubernetes.Data.Inmuebles;

public class InmuebleRepository : IInmuebleRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IUsuarioSesion _sesion;

    private readonly UserManager<Usuario> _userManager;
    public InmuebleRepository(ApplicationDbContext context, IUsuarioSesion sesion, UserManager<Usuario> userManager)
    {
        _context = context;
        _sesion = sesion;
        _userManager = userManager;
    }
    public async Task CreateInmueble(Inmueble inmueble)
    {
        var usuario = await _userManager.FindByNameAsync(_sesion.ObtenerUsuarioSesion());

        if (usuario is null)
        {
            throw new MiddlewareException(HttpStatusCode.Unauthorized, new { mensaje = "El usuario no es válido para hacer inserción." });
        }

        if (inmueble is null)
        {
            throw new MiddlewareException(HttpStatusCode.BadRequest, new { mensaje = "Datos de inmueble son incorrectos." });
        }
        inmueble.FechaCreacion = DateTime.Now;
        inmueble.UsuarioId = Guid.Parse(usuario!.Id);
        _context.Inmueble!.Add(inmueble);
    }

    public async Task DeleteInmueble(int id)
    {
        var inmueble = await _context.Inmueble!.FirstOrDefaultAsync(p => p.Id == id);
        _context.Inmueble!.Remove(inmueble!);
    }

    public async Task<IEnumerable<Inmueble>> GetAllInmuebles()
    {
        return await _context.Inmueble!.ToListAsync();
    }

    public async Task<Inmueble> GetInmuebleById(int id)
    {
        return await _context.Inmueble!.FirstOrDefaultAsync(p => p.Id == id)!;
    }

    public async Task<bool> SaveChanges()
    {
        return await _context.SaveChangesAsync() >= 0;
    }
}