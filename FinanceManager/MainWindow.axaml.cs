using Avalonia.Controls;
using Avalonia.Controls.Primitives;

namespace FinanceManager
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

       private void TcViews_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string tabItem = ((sender as TabControl).SelectedItem as TabItem).Header as string;

            switch (tabItem)
            {
                case "Finance":
                    break;

                case "Import":
                    break;

                case "Settings":
                    break;

                case "Console":
                    break;

                default:
                    return;
            }
        }
    }
}