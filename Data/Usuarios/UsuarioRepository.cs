using Microsoft.AspNetCore.Identity;
using NetKubernetes.Dtos.UsuarioDTO;
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
        return TransformUserToUserDTO(usuario!);
    }

    public async Task<UsuarioResponseDTO> Login(UsuarioLoginRequestDTO request)
    {
        var usuario = await _userManager.FindByEmailAsync(request.Email!);
        await _signInManager.CheckPasswordSignInAsync(usuario!, request.Password!, false);
        return TransformUserToUserDTO(usuario!);
    }

    public async Task<UsuarioResponseDTO> RegistroUsuario(UsuarioRegistroRequestDTO request)
    {
        var usuario = new Usuario
        {
            Nombre = request.Nombre,
            Apellido = request.Apellido,
            Telefono = request.Telefono,
            Email = request.Email,
            UserName = request.UserName
        };
        await _userManager.CreateAsync(usuario!, request.Password!);
        return TransformUserToUserDTO(usuario);
    }
}