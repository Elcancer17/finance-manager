using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManager.Utils
{
    public class FluentThemeExtentions
    {
        public const string TEXT = "TextControlForeground";
        public const string ERROR_TEXT = "SystemControlErrorTextForegroundBrush";
        public const string TEXT_ACCENT = "SystemControlForegroundAccentBrush";

        public const string BACKGROUND_LOWER = "SystemControlBackgroundListLowBrush";
        /// <summary>
        /// closest to background color
        /// </summary>
        public const string BACKGROUND_LOW = "SystemControlBackgroundBaseLowBrush";
        public const string BACKGROUND_MEDIUM_LOW = "SystemControlBackgroundBaseMediumLowBrush";
        public const string BACKGROUND_MEDIUM = "SystemControlBackgroundBaseMediumBrush";
        public const string BACKGROUND_MEDIUM_HIGH = "SystemControlBackgroundBaseMediumHighBrush";
        /// <summary>
        /// closest to text color
        /// </summary>
        public const string BACKGROUND_HIGH = "SystemControlBackgroundBaseHighBrush";
    }
}
