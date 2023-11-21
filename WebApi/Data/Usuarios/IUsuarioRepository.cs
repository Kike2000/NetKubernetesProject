using NetKubernetes.Dtos.UsuarioDTO;

namespace NetKubernetes.Data.Usuarios;

public interface IUsuarioRepository
{
    Task<UsuarioResponseDTO> GetUsuario();
    Task<UsuarioResponseDTO> Login(UsuarioLoginRequestDTO request);
    Task<UsuarioResponseDTO> RegistroUsuario(UsuarioRegistroRequestDTO request);
}