using Microsoft.AspNetCore.Identity;
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

        inmueble.FechaCreacion = DateTime.Now;
        inmueble.UsuarioId = Guid.Parse(usuario!.Id);
        _context.Inmueble!.Add(inmueble);
    }

    public void DeleteInmueble(int id)
    {
        var inmueble = _context.Inmueble!.FirstOrDefault(p => p.Id == id);
        _context.Inmueble!.Remove(inmueble!);
    }

    public IEnumerable<Inmueble> GetAllInmuebles()
    {
        return _context.Inmueble!.ToList();
    }

    public Inmueble GetInmuebleById(int id)
    {
        return _context.Inmueble!.FirstOrDefault(p => p.Id == id)!;
    }

    public bool SaveChanges()
    {
        return _context.SaveChanges() >= 0;
    }
}