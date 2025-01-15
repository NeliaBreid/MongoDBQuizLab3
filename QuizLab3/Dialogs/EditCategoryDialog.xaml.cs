using System.Windows;

namespace QuizLab3.Dialogs
{
    /// <summary>
    /// Interaction logic for EditCategoryDialog.xaml
    /// </summary>
    public partial class EditCategoryDialog : Window
    {
        public EditCategoryDialog()
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
