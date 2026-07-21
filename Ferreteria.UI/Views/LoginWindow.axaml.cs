using Avalonia.Input;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;
using Ferreteria.UI.ViewModels;

namespace Ferreteria.UI.Views;

public partial class LoginWindow : Window
{
    public LoginWindow()
    {
        InitializeComponent();
        var vm = new LoginViewModel();
        DataContext = vm;
        vm.SolicitarFocoUsuario += (s, e) =>
        {
            Dispatcher.UIThread.Post(() => txtUsuario.Focus());
        };

        Opened += (s, e) =>
        {
            Dispatcher.UIThread.Post(() => txtUsuario.Focus());
        };
        txtUsuario.KeyDown += TxtUsuario_KeyDown;
        txtContrasena.KeyDown += TxtContrasena_KeyDown;
    }
    private void TxtUsuario_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            txtContrasena.Focus();
        }
    }
    private void TxtContrasena_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            var vm = (LoginViewModel)DataContext!;
            if (vm.IngresarCommand.CanExecute(null))
            {
                vm.IngresarCommand.Execute(null);
            }
        }
    }
    private void BarraSuperior_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
        {
            BeginMoveDrag(e);
        }
    }
    private void BtnCerrar_Click(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}