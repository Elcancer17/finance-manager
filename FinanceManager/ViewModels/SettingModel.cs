using PropertyModels.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManager.ViewModels
{
    public class SettingModel : PropertyModels.ComponentModel.ReactiveObject
    {
        [PathBrowsable(pathSelection: PathBrowsableType.Directory, Title = "Select the working directory")]
        [DisplayName("Working Directory")]
        public string WorkingDirectory { get; set; }
    }
}
