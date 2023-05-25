namespace QuizHut.Views.UserControls.MainUserControls.QuizUserControls
{
    using System.Windows.Controls;

    public partial class QuizSettingsView : UserControl
    {
        public QuizSettingsView()
        {
            InitializeComponent();
        }

        private void Expander_Expanded(object sender, System.Windows.RoutedEventArgs e)
        {
            QSVQuestionsDataGrid.RowDetailsVisibilityMode = DataGridRowDetailsVisibilityMode.Collapsed;
        }

        private void Expander_Collapsed(object sender, System.Windows.RoutedEventArgs e)
        {
            QSVQuestionsDataGrid.RowDetailsVisibilityMode = DataGridRowDetailsVisibilityMode.VisibleWhenSelected;
        }
    }
}