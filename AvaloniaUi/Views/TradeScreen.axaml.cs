using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Engine.Models;
using Engine.ViewModels;
using MsBox.Avalonia;
using System.Threading.Tasks;
using System.Windows;

namespace AvaloniaUi;

public partial class TradeScreen : Window
{
    public GameSessionViewModel? SessionViewModel => DataContext as GameSessionViewModel;

    public TradeScreen()
    {
        InitializeComponent();
    }

    private void OnClick_Sell(object sender, RoutedEventArgs e)
    {
        if (sender is Control frameworkElement && frameworkElement.DataContext is GameItem item)
        {
            if (SessionViewModel != null)
            {
                SessionViewModel.CurrentPlayer.Gold += item.Price;
                SessionViewModel.CurrentTrader?.AddItemToInventory(item);
                SessionViewModel.CurrentPlayer.RemoveItemFromInventory(item);
            }
        }
    }

    private void OnClick_Buy(object sender, RoutedEventArgs e)
    {
        if (sender is Control frameworkElement && frameworkElement.DataContext is GameItem item)
        {
            if (SessionViewModel != null && SessionViewModel.CurrentPlayer.Gold >= item.Price)
            {
                SessionViewModel.CurrentPlayer.Gold -= item.Price;
                SessionViewModel.CurrentTrader?.RemoveItemFromInventory(item);
                SessionViewModel.CurrentPlayer.AddItemToInventory(item);
            }
            else
            {
                var msg = MessageBoxManager.GetMessageBoxStandard("Caption", "You do not have enough gold", MsBox.Avalonia.Enums.ButtonEnum.Ok);
                Task.FromResult(msg.ShowAsync());
            }
        }
    }

    private void OnClick_Close(object sender, RoutedEventArgs e)
    {
        Close();
    }
}