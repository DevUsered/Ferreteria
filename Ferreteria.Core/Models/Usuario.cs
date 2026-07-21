using System;

namespace Ferreteria.Core.Models;

public class Usuario
{
    public string Id { get; set; }
    
    public string NombreUsuario { get; set; } = string.Empty;
    
   
    public string PasswordHash { get; set; } = string.Empty;
    
    // Útil para tu ferretería: diferenciar entre "Admin" y "Empleado"
    public string Rol { get; set; } = "Empleado"; 
    
    // Para no borrar empleados si renuncian, solo los desactivamos
    public bool Activo { get; set; } = true; 
    
    public DateTime FechaCreacion { get; set; } = DateTime.Now;
}