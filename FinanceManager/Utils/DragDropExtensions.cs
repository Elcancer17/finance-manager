using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
