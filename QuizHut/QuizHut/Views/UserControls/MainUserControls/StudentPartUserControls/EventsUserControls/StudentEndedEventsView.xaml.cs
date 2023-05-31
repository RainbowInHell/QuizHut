namespace QuizHut.Views.UserControls.MainUserControls.StudentPartUserControls.EventsUserControls
{
    using System.Windows.Controls;

    public partial class StudentEndedEventsView : UserControl
    {
        public StudentEndedEventsView()
        {
            InitializeComponent();
        }

        private void Expander_Expanded(object sender, System.Windows.RoutedEventArgs e)
        {
            MSEEVActiveEventsDataGrid.RowDetailsVisibilityMode = DataGridRowDetailsVisibilityMode.Collapsed;
        }

        private void Expander_Collapsed(object sender, System.Windows.RoutedEventArgs e)
        {
            MSEEVActiveEventsDataGrid.RowDetailsVisibilityMode = DataGridRowDetailsVisibilityMode.VisibleWhenSelected;
        }
    }
}