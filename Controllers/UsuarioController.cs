using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetKubernetes.Data.Usuarios;
using NetKubernetes.Dtos.UsuarioDTO;

namespace NetKubernetes.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UsuarioController : ControllerBase
{
    private readonly IUsuarioRepository _repository;
    public UsuarioController(IUsuarioRepository repository)
    {
        _repository = repository;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<UsuarioResponseDTO>> Login([FromBody] UsuarioLoginRequestDTO request)
    {
        return await _repository.Login(request);
    }
    [AllowAnonymous]
    [HttpPost("registrar")]
    public async Task<ActionResult<UsuarioResponseDTO>> Registrar([FromBody] UsuarioRegistroRequestDTO request)
    {
        return await _repository.RegistroUsuario(request);
    }

    [HttpGet]
    public async Task<ActionResult<UsuarioResponseDTO>> ObtenerUsuario()
    {
        return await _repository.GetUsuario();
    }
}