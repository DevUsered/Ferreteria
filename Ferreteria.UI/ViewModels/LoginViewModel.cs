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
            ColorMensaje = "#D32F2F"; 
            MensajeError = "⚠️ Por favor ingresa tu usuario y contraseña.";
            SolicitarFocoUsuario?.Invoke(this, EventArgs.Empty);
            return;
        }
        
        EstaCargando = true;
        ColorMensaje = "#E6A800"; 
        MensajeError = "⏳ Verificando credenciales...";

        try
        {
            var usuarioEncontrado = await _usuarioService.AutenticarAsync(Usuario, Contrasena);
            
            if (usuarioEncontrado != null)
            {
                if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                {
                    // 1. Guardamos la referencia a la ventana actual (Login)
                    var ventanaLogin = desktop.MainWindow;
                    
                    // 2. Creamos la nueva ventana
                    var ventanaPrincipal = new MainWindow(usuarioEncontrado);
                    
                    // 3. ¡MUY IMPORTANTE! Le decimos a Avalonia que esta es la nueva jefa
                    desktop.MainWindow = ventanaPrincipal;
                    
                    // 4. Mostramos la nueva y cerramos la vieja de forma segura
                    ventanaPrincipal.Show();
                    ventanaLogin?.Close();
                }
                
                // 5. Salimos del método inmediatamente. El login ya murió.
                return; 
            }
            else
            {
                // Si falla, mostramos error y regresamos el foco
                ColorMensaje = "#D32F2F"; 
                MensajeError = "❌ Usuario o contraseña incorrectos.";
                Usuario = string.Empty;
                Contrasena = string.Empty;
                SolicitarFocoUsuario?.Invoke(this, EventArgs.Empty);
            }
        }
        catch (Exception ex)
        {
            MensajeError = $"Error de conexión: {ex.Message}";
            ColorMensaje = "#D32F2F";
        }

        // Si llegamos aquí, es porque falló el login o hubo un error de conexión
        EstaCargando = false;
    }
}