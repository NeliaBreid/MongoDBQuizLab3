using System.Windows;

namespace QuizLab3.Dialogs
{
    /// <summary>
    /// Interaction logic for PackOptionsDialog.xaml
    /// </summary>
    public partial class PackOptionsDialog : Window
    {
        public PackOptionsDialog()
        {
            InitializeComponent();
            DataContext = App.Current.MainWindow.DataContext;
        }
    }
}
