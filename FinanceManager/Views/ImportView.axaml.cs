using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using FinanceManager.Domain;
using FinanceManager.Utils;
using FinanceManager.ViewModels;
using System.Collections.Generic;
using System.Diagnostics;
using System;
using System.Linq;
using FinanceManager.Logging;
using FinanceManager.Import;
using static System.Net.WebRequestMethods;
using DynamicData;
using System.Collections.Specialized;

namespace FinanceManager.Views;

public partial class ImportView : UserControl
{
    List<FinancialTransaction> fts;
    FinancialTransactionManager ftm;

    private MainModel Model => DataContext as MainModel;
    public ImportView()
    {
        InitializeComponent();
        dgImportedData.AddDragDropHandler(Drop);
        //lcLogs.AddDragDropHandler(DragDropExtensions.Drop);
        ftm = new FinancialTransactionManager();
        fts = ftm.Load();
    }

    private void Drop(object sender, DragEventArgs e)
    {
        //lcLogs.ClearLogs();
        List<string> files = e.Data.GetFiles()?.Select(x => x.Path.LocalPath).ToList();
        if (files == null)//null whenever you didnt drop a file
        {
            return;
        }
        //do something here
        for (int i = 0; i < files.Count; i++)
        {
            Trace.WriteLine(files[i], LogLevel.Information.ToString());
            ImportManager im = new ImportManager(files[i]);
            fts = im.ImporFile(fts);
        }
        //Model.CreateDummyData();
        Model.LoadData(fts);
    }

    //Private void MainUserControl_GotFocus(ByVal sender as Object, ByVal e as EventArgs) Handles Me.GotFocus
    //MessageBox.Show("got focus")
    //End Sub

    //private void Focus(object sender, RoutedEventArgs e)
    //{
    //    string aaa = "";
    //}

    private void btnClear_Click(object sender, RoutedEventArgs e)
    {
        //lcLogs.ClearLogs();
    }
    private void btnSave_Click(object sender, RoutedEventArgs e)
    {
        ftm.Save(fts);
    }

    private void UserControl_DataContextChanged(object sender, EventArgs e)
    {
        if (DataContext is MainModel model)
        {
            model.Import.ImportedData.CollectionChanged -= ImportedData_CollectionChanged;

            model.Import.ImportedData.CollectionChanged += ImportedData_CollectionChanged;
        }
    }

    private void ImportedData_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        Model.Import.CalculateImportedDataTotals();
    }
}