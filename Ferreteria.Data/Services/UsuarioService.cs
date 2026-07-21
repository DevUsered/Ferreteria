using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Ferreteria.Core.Interfaces;
using Ferreteria.Core.Models;
using Ferreteria.Data.Context;

namespace Ferreteria.Data.Services;

public class UsuarioService : IUsuarioService
{
    public void InicializarBaseDatos()
    {
        using var dbContext = new AppDbContext();
        dbContext.Database.Migrate();

        if (!dbContext.Usuarios.Any())
        {
            dbContext.Usuarios.Add(new Usuario
            {
                Id = "EMP-001",
                NombreUsuario = "Admin",
                PasswordHash = "admin",
                Rol = "Admin"
            });
            dbContext.SaveChanges();
        }
    }
    public async Task<Usuario?> AutenticarAsync(string nombreUsuario, string contrasena)
    {
        using var dbContext = new AppDbContext();
        return await dbContext.Usuarios.FirstOrDefaultAsync(u => 
            u.NombreUsuario == nombreUsuario && 
            u.PasswordHash == contrasena && 
            u.Activo);
    }
}