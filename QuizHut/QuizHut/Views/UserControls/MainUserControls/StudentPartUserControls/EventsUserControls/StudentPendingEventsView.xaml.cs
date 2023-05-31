namespace QuizHut.Views.UserControls.MainUserControls.StudentPartUserControls.EventsUserControls
{
    using System.Windows.Controls;

    public partial class StudentPendingEventsView : UserControl
    {
        public StudentPendingEventsView()
        {
            InitializeComponent();
        }

        private void Expander_Expanded(object sender, System.Windows.RoutedEventArgs e)
        {
            MSPEVActiveEventsDataGrid.RowDetailsVisibilityMode = DataGridRowDetailsVisibilityMode.Collapsed;
        }

        private void Expander_Collapsed(object sender, System.Windows.RoutedEventArgs e)
        {
            MSPEVActiveEventsDataGrid.RowDetailsVisibilityMode = DataGridRowDetailsVisibilityMode.VisibleWhenSelected;
        }
    }
}