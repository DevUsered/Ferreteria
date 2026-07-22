using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Layout;
using Ferreteria.Core.Models;

namespace Ferreteria.UI.Views;

public partial class MainWindow : Window
{
    public MainWindow(Usuario usuarioLogueado)
    {
        InitializeComponent();
        
        // Simulación de sesión (ya que aún no hemos conectado el Usuario real aquí)
        lblNombreUsuario.Text = usuarioLogueado.NombreUsuario;
        lblRolUsuario.Text = usuarioLogueado.Rol.ToUpper();

        if (usuarioLogueado.Rol != "Administrador" && usuarioLogueado.Rol != "Admin")
        {
            panelAdmin.IsVisible = false;
        }
        
        CargarVista("Punto de Venta", "🛒");
    }

    // --- CONTROLES DE VENTANA ---
    public MainWindow()
    {
        InitializeComponent();
        
        lblNombreUsuario.Text = "Modo Diseño";
        lblRolUsuario.Text = "ADMIN";
    }
    private void BarraSuperior_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        var punto = e.GetCurrentPoint(this);

        if (punto.Properties.IsLeftButtonPressed)
        {
            // Si el usuario hace doble clic rápido
            if (e.ClickCount == 2)
            {
                // Reutilizamos tu método de maximizar
                btnMaximizar_Click(sender, e);
            }
            else
            {
                // Si es un solo clic y mantiene presionado, arrastramos
                BeginMoveDrag(e);
            }
        }
    }

    private void btnMinimizar_Click(object? sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }

    private void btnMaximizar_Click(object? sender, RoutedEventArgs e)
    {
        if (WindowState == WindowState.Maximized)
        {
            WindowState = WindowState.Normal;
            btnMaximizar.Content = "□";
        }
        else
        {
            WindowState = WindowState.Maximized;
            btnMaximizar.Content = "❐";
        }
    }

    private void btnCerrarApp_Click(object? sender, RoutedEventArgs e)
    {
        Close();
    }

    private void btnCerrarSesion_Click(object? sender, RoutedEventArgs e)
    {
        // TODO: Agregar cuadro de diálogo de confirmación aquí
        var login = new LoginWindow();
        login.Show();
        Close();
    }

    // --- OCULTAR/MOSTRAR SIDEBAR ---
    private void btnToggleSidebar_Click(object? sender, RoutedEventArgs e)
    {
       
        if (SidebarContainer.IsVisible)
        {
            SidebarContainer.IsVisible = false;
            btnToggleSidebar.Content = "▶";
        }
        else
        {
            SidebarContainer.IsVisible = true;
            btnToggleSidebar.Content = "☰";
        }
    }

    // --- NAVEGACIÓN DINÁMICA ---
    private void btnNav_IsCheckedChanged(object? sender, RoutedEventArgs e)
    {
        if (sender is RadioButton btn && btn.IsChecked == true)
        {
            CargarVista(btn.Content?.ToString() ?? "", btn.Tag?.ToString() ?? "");
        }
    }

    private void CargarVista(string titulo, string icono)
    {
        if (lblTituloSeccion == null || lblIconoSeccion == null || AreaDeTrabajo == null) return;

        lblTituloSeccion.Text = titulo;
        lblIconoSeccion.Text = icono;

        // Renderizado provisional para que pruebes los clics
        var panelProvisional = new StackPanel 
        { 
            VerticalAlignment = VerticalAlignment.Center, 
            HorizontalAlignment = HorizontalAlignment.Center 
        };

        panelProvisional.Children.Add(new TextBlock
        {
            Text = icono,
            FontSize = 64,
            HorizontalAlignment = HorizontalAlignment.Center,
            Margin = new Avalonia.Thickness(0, 0, 0, 15),
            FontFamily = FontFamily.Parse("Segoe UI Emoji, Noto Color Emoji")
        });

        panelProvisional.Children.Add(new TextBlock
        {
            Text = $"Sección de {titulo}",
            FontSize = 28,
            FontWeight = FontWeight.Black,
            Foreground = new SolidColorBrush(Avalonia.Media.Color.Parse("#2C2825")),
            HorizontalAlignment = HorizontalAlignment.Center
        });

        AreaDeTrabajo.Content = panelProvisional;
    }
}