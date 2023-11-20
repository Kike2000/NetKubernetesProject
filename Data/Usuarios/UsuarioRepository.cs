using System.Net;
using Microsoft.AspNetCore.Identity;
using NetKubernetes.Dtos.UsuarioDTO;
using NetKubernetes.Middleware;
using NetKubernetes.Models;
using NetKubernetes.Token;

namespace NetKubernetes.Data.Usuarios;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly UserManager<Usuario> _userManager;
    private readonly SignInManager<Usuario> _signInManager;
    private readonly IJwtGenerator _jwtGenerator;
    private readonly ApplicationDbContext _context;
    private readonly IUsuarioSesion _usuarioSesion;

    public UsuarioRepository(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager,
                            IJwtGenerator jwtGenerator, ApplicationDbContext context, IUsuarioSesion usuarioSesion)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtGenerator = jwtGenerator;
        _context = context;
        _usuarioSesion = usuarioSesion;
    }
    private UsuarioResponseDTO TransformUserToUserDTO(Usuario usuario)
    {
        return new UsuarioResponseDTO
        {
            Id = usuario.Id,
            Nombre = usuario.Nombre,
            Apellido = usuario.Apellido,
            Telefono = usuario.Telefono,
            Email = usuario.Email,
            UserName = usuario.UserName,
            Token = _jwtGenerator.CrearToken(usuario)
        };
    }

    public async Task<UsuarioResponseDTO> GetUsuario()
    {
        var usuario = await _userManager.FindByNameAsync(_usuarioSesion.ObtenerUsuarioSesion());
        if (usuario is null)
        {
            throw new MiddlewareException(HttpStatusCode.Unauthorized, new { mensaje = "El token del usuario no existe en la bd." });
        }
        return TransformUserToUserDTO(usuario!);
    }

    public async Task<UsuarioResponseDTO> Login(UsuarioLoginRequestDTO request)
    {
        var usuario = await _userManager.FindByEmailAsync(request.Email!);
        if (usuario is null)
        {
            throw new MiddlewareException
            (HttpStatusCode.Unauthorized, new { mensaje = "El email del usuario no existe en la bd." });
        }
        var resultado = await _signInManager.CheckPasswordSignInAsync(usuario!, request.Password!, false);
        if (resultado.Succeeded)
        {
            return TransformUserToUserDTO(usuario!);
        }
        throw new MiddlewareException(HttpStatusCode.Unauthorized, new { mensaje = "Las credenciales son incorrectas." });
    }

    public async Task<UsuarioResponseDTO> RegistroUsuario(UsuarioRegistroRequestDTO request)
    {
        var existeEmail = _context.Users.Any(p => p.Email == request.Email);
        if (existeEmail)
        {
            throw new MiddlewareException
            (HttpStatusCode.BadRequest, new { mensaje = "El usuario ya existe en la bd." });
        }
        var existeUserName = _context.Users.Any(p => p.Email == request.UserName);
        if (existeEmail)
        {
            throw new MiddlewareException
            (HttpStatusCode.BadRequest, new { mensaje = "El UserName ya existe en la bd." });
        }

        var usuario = new Usuario
        {
            Nombre = request.Nombre,
            Apellido = request.Apellido,
            Telefono = request.Telefono,
            Email = request.Email,
            UserName = request.UserName
        };
        var resultado = await _userManager.CreateAsync(usuario!, request.Password!);
        if (resultado.Succeeded)
        {
            return TransformUserToUserDTO(usuario!);
        }
        throw new Exception("No se pudo registrar el usuario.");
    }
}