﻿using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using FinanceManager.Import;
using FinanceManager.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FinanceManager.Import.FileManager;
using static FinanceManager.Import.QuickenManager;

namespace FinanceManager.Utils
{
    public static class DragDropExtensions
    {
        public static void AddDragDropHandler(this Interactive control, EventHandler<DragEventArgs> dropHandler, EventHandler <DragEventArgs> dragOverHanlder = default)
        {
            DragDrop.SetAllowDrop(control, true);
            control.AddHandler(DragDrop.DropEvent, dropHandler);
            if(dragOverHanlder != null)
            {
                control.AddHandler(DragDrop.DragOverEvent, dragOverHanlder);
            }
            else
            {
                control.AddHandler(DragDrop.DragOverEvent, DefaultDragOverHandler);
            }
        }

        private static void DefaultDragOverHandler(object sender, DragEventArgs e)
        {
            e.DragEffects = DragDropEffects.Move;
        }
    }
}
