namespace QuizHut.Infrastructure.Services
{
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;

    using Microsoft.EntityFrameworkCore;

    using OfficeOpenXml;

    using QuizHut.DLL.Entities;
    using QuizHut.DLL.Repositories.Contracts;
    using QuizHut.Infrastructure.EntityViewModels;
    using QuizHut.Infrastructure.EntityViewModels.Categories;
    using QuizHut.Infrastructure.EntityViewModels.Groups;
    using QuizHut.Infrastructure.EntityViewModels.Quizzes;
    using QuizHut.Infrastructure.Services.Contracts;

    public class Exporter : IExporter
    {
        private readonly IRepository<Result> repository;

        public Exporter(IRepository<Result> repository)
        {
            this.repository = repository;
        }

        public void GenerateExcelReport()
        {
            // Fetch all the results with related event, student, and group data
            var resultsWithEvents = repository.All()
                .Include(r => r.Event)
                    .ThenInclude(e => e.EventsGroups)
                        .ThenInclude(eg => eg.Group)
                .Include(r => r.Student)
                .ToList();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            // Create a new Excel package
            using (var package = new ExcelPackage())
            {
                // Create the worksheet
                var worksheet = package.Workbook.Worksheets.Add("Report");

                // Set the column headers
                worksheet.Cells[1, 1].Value = "Название события";
                worksheet.Cells[1, 2].Value = "Группа";
                worksheet.Cells[1, 3].Value = "Викторина";
                worksheet.Cells[1, 4].Value = "Студент";
                worksheet.Cells[1, 5].Value = "Баллы";
                worksheet.Cells[1, 6].Value = "Максимально баллов";
                worksheet.Cells[1, 7].Value = "Время прохождения";
                worksheet.Cells[1, 8].Value = "Дата";

                worksheet.Cells[1, 1, 1, 8].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                // Populate the data rows
                for (var i = 0; i < resultsWithEvents.Count; i++)
                {
                    var result = resultsWithEvents[i];

                    var eventName = result.Event.Name;
                    var groupName = result.Event.EventsGroups.FirstOrDefault()?.Group.Name; // Get the group name if available
                    var quizName = result.QuizName;
                    var studentName = $"{result.Student.FirstName} {result.Student.LastName}";
                    var points = result.Points;
                    var maxPoints = result.MaxPoints;
                    var timeSpent = result.TimeSpent.ToString(@"hh\:mm\:ss");
                    var activationDate = result.Event.ActivationDateAndTime.ToShortDateString();

                    worksheet.Cells[i + 2, 1].Value = eventName;
                    worksheet.Cells[i + 2, 2].Value = groupName;
                    worksheet.Cells[i + 2, 3].Value = quizName;
                    worksheet.Cells[i + 2, 4].Value = studentName;
                    worksheet.Cells[i + 2, 5].Value = points;
                    worksheet.Cells[i + 2, 6].Value = maxPoints;
                    worksheet.Cells[i + 2, 7].Value = timeSpent;
                    worksheet.Cells[i + 2, 8].Value = activationDate;

                    worksheet.Cells[i + 2, 1, i + 2, 8].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                }

                // Auto-fit columns for better readability
                worksheet.Cells.AutoFitColumns();

                // Save the Excel package to a file
                var filePath = @"D:\Report.xlsx";
                package.SaveAs(new FileInfo(filePath));
            }
        }

        public void GenerateExcelReport(ObservableCollection<StudentViewModel> students)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            // Create a new Excel package
            using (var package = new ExcelPackage())
            {
                // Create the worksheet
                var worksheet = package.Workbook.Worksheets.Add("Report");

                // Set the column headers
                worksheet.Cells[1, 1].Value = "Id";
                worksheet.Cells[1, 2].Value = "Студент";
                worksheet.Cells[1, 3].Value = "Электронная  почта";

                // Set center alignment for column headers
                worksheet.Cells[1, 1, 1, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                // Populate the data rows
                for (var i = 0; i < students.Count; i++)
                {
                    var student = students[i];

                    worksheet.Cells[i + 2, 1].Value = student.Id;
                    worksheet.Cells[i + 2, 2].Value = student.FullName;
                    worksheet.Cells[i + 2, 3].Value = student.Email;

                    // Set center alignment for data cells
                    worksheet.Cells[i + 2, 1, i + 2, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                }

                // Auto-fit columns for better readability
                worksheet.Cells.AutoFitColumns();

                // Save the Excel package to a file
                var filePath = @"D:\Report.xlsx";
                package.SaveAs(new FileInfo(filePath));
            }
        }

        public void GenerateExcelReport(ObservableCollection<QuizListViewModel> quizzes)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            // Create a new Excel package
            using (var package = new ExcelPackage())
            {
                // Create the worksheet
                var worksheet = package.Workbook.Worksheets.Add("Report");

                // Set the column headers
                worksheet.Cells[1, 1].Value = "Id";
                worksheet.Cells[1, 2].Value = "Название";
                worksheet.Cells[1, 3].Value = "Количество вопросов";
                worksheet.Cells[1, 4].Value = "Название категории";
                worksheet.Cells[1, 5].Value = "Описание";
                worksheet.Cells[1, 6].Value = "Пароль";
                worksheet.Cells[1, 7].Value = "Id события";
                worksheet.Cells[1, 8].Value = "Время на прохождение";
                worksheet.Cells[1, 9].Value = "Вопросы";

                // Set center alignment for column headers
                worksheet.Cells[1, 1, 1, 9].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                // Populate the data rows
                int maxQuestionLength = 0;
                for (var i = 0; i < quizzes.Count; i++)
                {
                    var quiz = quizzes[i];

                    worksheet.Cells[i + 2, 1].Value = quiz.Id;
                    worksheet.Cells[i + 2, 2].Value = quiz.Name;
                    worksheet.Cells[i + 2, 3].Value = quiz.QuestionsCount;
                    worksheet.Cells[i + 2, 4].Value = quiz.CategoryName;
                    worksheet.Cells[i + 2, 5].Value = quiz.Description;
                    worksheet.Cells[i + 2, 6].Value = quiz.Password;
                    worksheet.Cells[i + 2, 7].Value = quiz.EventId;
                    worksheet.Cells[i + 2, 8].Value = quiz.Timer;

                    worksheet.Cells[i + 2, 9].Value = string.Join("\n", quiz.Questions.Select(q => q.Text));
                    worksheet.Cells[i + 2, 9].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Cells[i + 2, 9].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    worksheet.Cells[i + 2, 9].Style.WrapText = true;

                    int maxQuestionLengthInQuiz = quiz.Questions.Max(q => q.Text.Length);
                    if (maxQuestionLengthInQuiz > maxQuestionLength)
                        maxQuestionLength = maxQuestionLengthInQuiz;
                }

                // Auto-fit columns for better readability
                worksheet.Cells.AutoFitColumns();

                // Set the column width for the question column based on the longest question
                int questionColumnWidth = maxQuestionLength * 1; // Adjust the multiplier as needed
                worksheet.Column(9).Width = questionColumnWidth;

                // Save the Excel package to a file
                var filePath = @"D:\Report.xlsx";
                package.SaveAs(new FileInfo(filePath));
            }
        }

        public void GenerateExcelReport(ObservableCollection<CategoryViewModel> categories)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Report");

                worksheet.Cells[1, 1].Value = "Id";
                worksheet.Cells[1, 2].Value = "Название";
                worksheet.Cells[1, 3].Value = "Количество викторин";

                worksheet.Cells[1, 1, 1, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                int rowIndex = 2;
                for (var i = 0; i < categories.Count; i++)
                {
                    var category = categories[i];

                    worksheet.Cells[rowIndex, 1].Value = category.Id;
                    worksheet.Cells[rowIndex, 2].Value = category.Name;
                    worksheet.Cells[rowIndex, 3].Value = category.QuizzesCount;

                    rowIndex++;
                }

                worksheet.Cells.AutoFitColumns();

                worksheet.Cells[1, 1, rowIndex - 1, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Cells[1, 1, rowIndex - 1, 3].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                var filePath = @"D:\Report.xlsx";
                package.SaveAs(new FileInfo(filePath));
            }
        }

        public void GenerateExcelReport(ObservableCollection<GroupListViewModel> groups)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            // Create a new Excel package
            using (var package = new ExcelPackage())
            {
                // Create the worksheet
                var worksheet = package.Workbook.Worksheets.Add("Report");

                // Set the column headers
                worksheet.Cells[1, 1].Value = "Id";
                worksheet.Cells[1, 2].Value = "Название";
                worksheet.Cells[1, 3].Value = "Students";

                // Set center alignment for column headers
                worksheet.Cells[1, 1, 1, 7].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                // Populate the data rows
                int rowIndex = 2;
                for (var i = 0; i < groups.Count; i++)
                {
                    var group = groups[i];

                    worksheet.Cells[rowIndex, 1].Value = group.Id;
                    worksheet.Cells[rowIndex, 2].Value = group.Name;

                    // Set center alignment for the data cells
                    worksheet.Cells[rowIndex, 1, rowIndex, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                    // Add students for the group
                    var students = string.Join(", ", group.StudentsGroups.Select(sg => $"{sg.Student.FirstName} {sg.Student.LastName}"));
                    worksheet.Cells[rowIndex, 3].Value = students;

                    rowIndex++;
                }

                // Auto-fit columns for better readability
                worksheet.Cells.AutoFitColumns();

                // Set center alignment for all cells
                worksheet.Cells[1, 1, rowIndex - 1, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Cells[1, 1, rowIndex - 1, 3].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                // Save the Excel package to a file
                var filePath = @"D:\Report.xlsx";
                package.SaveAs(new FileInfo(filePath));
            }
        }
    }
}