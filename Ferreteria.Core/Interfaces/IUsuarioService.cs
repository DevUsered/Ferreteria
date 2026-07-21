using System.Threading.Tasks;
using Ferreteria.Core.Models;

namespace Ferreteria.Core.Interfaces;

public interface IUsuarioService
{
    // Método para crear la BD al inicio
    void InicializarBaseDatos(); 
    
    // Método para el login
    Task<Usuario?> AutenticarAsync(string nombreUsuario, string contrasena);
}