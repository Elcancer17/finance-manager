using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System;

namespace FinanceManager.Views;

public partial class SettingView : UserControl
{
    public SettingView()
    {
        InitializeComponent();
    }

    private void UserControl_DataContextChanged(object sender, EventArgs e)
    {

    }

    private void btnExport_Click(object sender, RoutedEventArgs e)
    {
    }
}