using System.Windows;

namespace lets_chat
{
    /// <summary>
    /// Interaction logic for AppView.xaml
    /// </summary>
    public partial class AppView : Window
    {
        public AppView(AppViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
