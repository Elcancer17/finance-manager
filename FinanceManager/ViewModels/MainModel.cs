using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManager.ViewModels
{
    internal class MainModel : ReactiveUI.ReactiveObject
    {
        public SettingModel Settings { get; set; } = new();
        public FinanceModel FinanceData { get; set; } = new();
    }
}
