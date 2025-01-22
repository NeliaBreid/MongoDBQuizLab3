using QuizLab3.Data;
using QuizLab3.ViewModel;
using System.Windows;

namespace QuizLab3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowViewModel viewModel;
        public MainWindow()
        {
            InitializeComponent();

            DataContext = viewModel = new MainWindowViewModel();
            
        }
    }
}