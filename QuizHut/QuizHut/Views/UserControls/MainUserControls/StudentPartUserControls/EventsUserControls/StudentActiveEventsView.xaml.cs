namespace QuizHut.Views.UserControls.MainUserControls.StudentPartUserControls.EventsUserControls
{
    using System.Windows.Controls;

    public partial class StudentActiveEventsView : UserControl
    {
        public StudentActiveEventsView()
        {
            InitializeComponent();
        }

        private void Expander_Expanded(object sender, System.Windows.RoutedEventArgs e)
        {
            MSAEVActiveEventsDataGrid.RowDetailsVisibilityMode = DataGridRowDetailsVisibilityMode.Collapsed;
        }

        private void Expander_Collapsed(object sender, System.Windows.RoutedEventArgs e)
        {
            MSAEVActiveEventsDataGrid.RowDetailsVisibilityMode = DataGridRowDetailsVisibilityMode.VisibleWhenSelected;
        }
    }
}