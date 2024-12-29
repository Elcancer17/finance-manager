using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.PropertyGrid.Controls;
using FinanceManager.Utils;
using FinanceManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FinanceManager.Views;

public partial class FinanceView : UserControl
{
    public FinanceView()
    {
        InitializeComponent();
        dgFinancialData.AddDragDropHandler(Drop);
    }

    private void Drop(object sender, DragEventArgs e)
    {
        List<string> files = e.Data.GetFiles()?.Select(x => x.Path.LocalPath).ToList();
        if(files == null)
        {
            return;
        }
        //do something here
    }
}