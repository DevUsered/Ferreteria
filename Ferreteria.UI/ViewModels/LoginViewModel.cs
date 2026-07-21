using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Ferreteria.UI.Views;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Ferreteria.Data.Services;

namespace Ferreteria.UI.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    private readonly UsuarioService _usuarioService = new UsuarioService();
    public event EventHandler? SolicitarFocoUsuario;

    [ObservableProperty]
    private string _usuario = string.Empty;
    
    [ObservableProperty]
    private string _contrasena = string.Empty;

    [ObservableProperty]
    private string _mensajeError = string.Empty;

    [ObservableProperty] 
    private string _colorMensaje = "#D32F2F";
    
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(IngresarCommand))]
    private bool _estaCargando = false;

    private bool PuedeIngresar() => !EstaCargando;
    
    [ObservableProperty] 
    private bool _mostrarContrasena = false;

    [RelayCommand]
    private void AlternarVisibilidadContrasena()
    {
        MostrarContrasena = !MostrarContrasena;
    }

    [RelayCommand(CanExecute = nameof(PuedeIngresar))]
    private async Task IngresarAsync()
    {
        MensajeError = string.Empty;

        if (string.IsNullOrWhiteSpace(Usuario) || string.IsNullOrWhiteSpace(Contrasena))
        {
            ColorMensaje = "#D32F2F"; // Rojo
            MensajeError = "⚠️ Por favor ingresa tu usuario y contraseña.";
            SolicitarFocoUsuario?.Invoke(this, EventArgs.Empty);
            return;
        }
        
        EstaCargando = true;
        ColorMensaje = "#E6A800"; // Naranja/Amarillo
        MensajeError = "⏳ Verificando credenciales...";

        bool loginExitoso = false;
        try
        {
            var usuarioEncontrado = await _usuarioService.AutenticarAsync(Usuario, Contrasena);
            
            if (usuarioEncontrado != null)
            {
                MensajeError = "¡Éxito! Ingresando...";
                if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                {
                    var ventanaPrincipal = new MainWindow();
                    ventanaPrincipal.Show();
                    desktop.MainWindow?.Close();
                    desktop.MainWindow = ventanaPrincipal;
                }
                loginExitoso = true;
            }
        }
        catch (Exception ex)
        {
            MensajeError = $"Error de conexión: {ex.Message}";
            ColorMensaje = "#D32F2F";
            EstaCargando = false;
            return;
        }

        if (loginExitoso)
        {
            MensajeError = "¡Éxito!";
            ColorMensaje = "#2E7D32"; 
        }
        else
        {
            ColorMensaje = "#D32F2F"; // Rojo
            MensajeError = "❌ Usuario o contraseña incorrectos.";
            Usuario = string.Empty;
            Contrasena = string.Empty;
            SolicitarFocoUsuario?.Invoke(this, EventArgs.Empty);
        }

        EstaCargando = false;
    }
}