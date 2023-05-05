namespace QuizHut.Views.UserControls
{
    using System.Collections.ObjectModel;
    using System.Windows.Controls;

    public partial class StudentsView : UserControl
    {
        public StudentsView()
        {
            InitializeComponent();

            ObservableCollection<Student> students = new ObservableCollection<Student>
            {
                new Student { Number = "1", Name = "Клавдия Ольгердовна", Email = "jkqvm@carpenter.com", Event = "Test 1" },
                new Student { Number = "2", Name = "Парфянович Алекснадр", Email = "lbrdn@orphan.com", Event = "Test 1" },
                new Student { Number = "3", Name = "Быковский Матвей", Email = "xnbty@shaman.com", Event = "Test 2" },
                new Student { Number = "4", Name = "Климченя Владимир", Email = "fjdge@allegory.com", Event = "Test 3" },
                new Student { Number = "5", Name = "Раткевич Денис", Email = "pzmbt@eclipse.com", Event = "Test 1" },
                new Student { Number = "6", Name = "Вишневский Владислав", Email = "kvtcl@tremble.com", Event = "Test 4" },
                new Student { Number = "7", Name = "Миклашеввич Илья", Email = "njfhl@wildfire.com", Event = "Test 1" },
                new Student { Number = "8", Name = "Григоренко Даниил", Email = "dhqsn@bronze.com", Event = "Test 2" }
            };

            MVSStudentDataGrid.ItemsSource = students;
        }
    }

    public class Student
    {
        public string Number { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Event { get; set; }
    }
}