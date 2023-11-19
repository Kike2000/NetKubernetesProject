using NetKubernetes.Dtos.UsuarioDTO;
using NetKubernetes.Models;

namespace NetKubernetes.Data.Usuarios;

public class UsuarioRepository : IUsuarioRepository
{
    public Task<UsuarioResponseDTO> GetUsuario()
    {
        throw new NotImplementedException();
    }

    public Task<UsuarioResponseDTO> Login(UsuarioLoginRequestDTO request)
    {
        throw new NotImplementedException();
    }

    public Task<UsuarioResponseDTO> RegistroUsuario(UsuarioRegistroRequestDTO request)
    {
        throw new NotImplementedException();
    }
}