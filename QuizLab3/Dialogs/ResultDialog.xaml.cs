using System.Windows;

namespace QuizLab3.Dialogs
{
    /// <summary>
    /// Interaction logic for ResultDialog.xaml
    /// </summary>
    public partial class ResultDialog : Window
    {
        public ResultDialog()
        {
            InitializeComponent();
            DataContext = App.Current.MainWindow.DataContext;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
