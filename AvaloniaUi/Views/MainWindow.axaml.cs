using Avalonia.Controls;
using AvRichTextBox;
using Engine.EventArgs;
using Engine.ViewModels;
using System;

namespace AvaloniaUi.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContextChanged += MainWindow_DataContextChanged;
            if (DataContext is GameSessionViewModel vm)
            {
                vm.OnMessageRaised += Vm_OnMessageRaised;
                vm.TradeCommand.Subscribe(_ =>
                {
                    var tradeScreen = new TradeScreen
                    {
                        DataContext = vm
                    };
                    tradeScreen.Show();
                });
            }
        }
        private void MainWindow_DataContextChanged(object? sender, EventArgs e)
        {
            if (DataContext is GameSessionViewModel vm)
            {
                vm.OnMessageRaised += Vm_OnMessageRaised;
                vm.TradeCommand.Subscribe(_ =>
                {
                    var tradeScreen = new TradeScreen
                    {
                        DataContext = vm
                    };
                    tradeScreen.Show();
                });
            }
        }

        private void Vm_OnMessageRaised(object? sender, GameMessageEventArgs e)
        {
            var editableRun = new EditableRun(e.Message); // EditableRun expects a string
            GameMessages.FlowDocument.Blocks.Add(new Paragraph { Inlines = { editableRun } });
        }
    }
}