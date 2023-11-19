using Microsoft.AspNetCore.Identity;
using NetKubernetes.Models;

namespace NetKubernetes.Data;

public class LoadDatabase
{
    public static async Task InsertarData(ApplicationDbContext context, UserManager<Usuario> userManager)
    {
        if (!userManager.Users.Any())
        {
            var usuario = new Usuario
            {
                Nombre = "Pedro",
                Apellido = "Rguez",
                Email = "pedro@gmail.com",
                UserName = "Pedro.enrique",
                Telefono = "26545485"
            };
            await userManager.CreateAsync(usuario, "Passwords123*");
        }
        if (!context.Inmueble!.Any())
        {
            context.Inmueble.AddRange(
                new Inmueble
                {
                    Nombre = "Casa de playa",
                    Direccion = "Av. El sol 32",
                    Precio = 23M,
                    FechaCreacion = DateTime.Now
                },
                new Inmueble
                {
                    Nombre = "Casa de Invierno",
                    Direccion = "Av. El sol 33",
                    Precio = 2M,
                    FechaCreacion = DateTime.Now
                }
            );
        }
        context.SaveChanges();
    }
}