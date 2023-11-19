using NetKubernetes.Models;

namespace NetKubernetes.Token;

public interface IJwtGenerator
{
    string CrearToken(Usuario usuario);
}