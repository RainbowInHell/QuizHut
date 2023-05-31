namespace QuizHut.Infrastructure.Services
{
    using System;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using OfficeOpenXml;
    using OfficeOpenXml.Style;
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

        private readonly IRepository<Event> eventRepository;

        private readonly IRepository<Quiz> quizRepository;

        private readonly IRepository<Category> categoryRepository;

        private readonly IRepository<Group> groupRepository;

        private readonly IRepository<ApplicationUser> studentRepository;

        public Exporter(
            IRepository<Result> repository, 
            IRepository<Event> eventRepository, 
            IRepository<Quiz> quizRepository,
            IRepository<Category> categoryRepository,
            IRepository<Group> groupRepository,
            IRepository<ApplicationUser> studentRepository)
        {
            this.repository = repository;
            this.eventRepository = eventRepository;
            this.quizRepository = quizRepository;
            this.categoryRepository = categoryRepository;
            this.groupRepository = groupRepository;
            this.studentRepository = studentRepository;
        }

        public async Task GenerateComplexResultsExcelReportAsync()
        {
            var resultsWithEventsAndQuizzes = await repository.All()
                .Include(r => r.Student)
                .Include(r => r.Quiz)
                    .ThenInclude(q => q.Event)
                        .ThenInclude(e => e.EventsGroups)
                            .ThenInclude(eg => eg.Group)
                .ToListAsync();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Better Report");

                worksheet.Cells[1, 1].Value = "Событие";
                worksheet.Cells[1, 2].Value = "Группы";
                worksheet.Cells[1, 3].Value = "Студент";
                worksheet.Cells[1, 4].Value = "Баллы";
                worksheet.Cells[1, 5].Value = "Максимально баллов";
                worksheet.Cells[1, 6].Value = "Зтраченное время";
                worksheet.Cells[1, 7].Value = "Дата";
                worksheet.Cells[1, 8].Value = "Викторина";

                worksheet.Cells[1, 1, 1, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                for (int i = 0; i < resultsWithEventsAndQuizzes.Count; i++)
                {
                    var result = resultsWithEventsAndQuizzes[i];
                    var eventName = result.Quiz?.Event?.Name;
                    var groupName = result.Quiz?.Event?.EventsGroups?.FirstOrDefault()?.Group?.Name;
                    var studentName = $"{result.Student?.FirstName} {result.Student?.LastName}";
                    var points = result.Points;
                    var maxPoints = result.MaxPoints;
                    var timeSpent = result.TimeSpent.ToString(@"hh\:mm\:ss");
                    var activationDate = result.Quiz?.Event?.ActivationDateAndTime.ToShortDateString();
                    var quizName = result.Quiz?.Name;

                    worksheet.Cells[i + 2, 1].Value = eventName;
                    worksheet.Cells[i + 2, 2].Value = groupName;
                    worksheet.Cells[i + 2, 3].Value = studentName;
                    worksheet.Cells[i + 2, 4].Value = points;
                    worksheet.Cells[i + 2, 5].Value = maxPoints;
                    worksheet.Cells[i + 2, 6].Value = timeSpent;
                    worksheet.Cells[i + 2, 7].Value = activationDate;
                    worksheet.Cells[i + 2, 8].Value = quizName;

                    worksheet.Cells[i + 2, 1, i + 2, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                worksheet.Cells.AutoFitColumns();

                var byteArray = await package.GetAsByteArrayAsync();
                await File.WriteAllBytesAsync(@"D:\ComplexResultsReport.xlsx", byteArray);
            }
        }

        public async Task GenerateStudentPerformanceDistributionByCategoryAsync()
        {
            var quizzes = await quizRepository.All().Include(q => q.Results).ToListAsync();

            var reportData = quizzes
                .SelectMany(q => q.Results.Select(r => new
                {
                    CategoryId = q.CategoryId,
                    CategoryName = categoryRepository.All().Where(x => x.Id == q.CategoryId).FirstOrDefault()?.Name,
                    StudentScore = r.Points
                }))
                .GroupBy(r => r.CategoryId)
                .Select(group => new
                {
                    CategoryId = group.Key,
                    CategoryName = group.FirstOrDefault()?.CategoryName,
                    ScoreDistribution = group.Select(r => Math.Round(r.StudentScore,2))
                })
                .ToList();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            // Generate the statistical report based on the data
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Student Performance Distribution");

                // Add headers
                worksheet.Cells[1, 1].Value = "Категория";
                worksheet.Cells[1, 2].Value = "Распределение баллов";

                worksheet.Cells[1, 1, 1, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                // Add data
                for (int i = 0; i < reportData.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = reportData[i].CategoryName;
                    worksheet.Cells[i + 2, 2].Value = string.Join(", ", reportData[i].ScoreDistribution);

                    worksheet.Cells[i + 2, 1, i + 2, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }


                // Save the Excel file
                await package.SaveAsAsync(new FileInfo(@"D:\StudentPerformanceDistribution.xlsx"));
            }
        }

        public async Task GenerateTimeSpentOnQuizzesByStudentAsync()
        {
            var students = await studentRepository.All().Include(s => s.Results).ToListAsync();

            var reportData = students
                .Select(s => new
                {
                    StudentName = $"{s.FirstName} {s.LastName}",
                    Results = s.Results.ToList() // Load the related Results into memory
                })
                .ToList()
                .Select(s => new
                {
                    s.StudentName,
                    TotalTimeSpent = s.Results.Sum(r => r.TimeSpent.TotalMinutes)
                })
                .ToList();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            // Generate the statistical report based on the data
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Time Spent on Quizzes by Student");

                // Add headers
                worksheet.Cells[1, 1].Value = "Student";
                worksheet.Cells[1, 2].Value = "Total Time Spent (minutes)";

                worksheet.Cells[1, 1, 1, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                // Add data
                for (int i = 0; i < reportData.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = reportData[i].StudentName;
                    worksheet.Cells[i + 2, 2].Value = reportData[i].TotalTimeSpent;

                    worksheet.Cells[i + 2, 1, i + 2, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                // Save the Excel file
                await package.SaveAsAsync(new FileInfo(@"D:\TimeSpentOnQuizzesByStudent.xlsx"));
            }
        }

        public async Task GenerateQuizCompletionRateByGroupAsync()
        {
            var groups = groupRepository.All().Include(g => g.StudentsGroups);

            var reportData = groups
                .Select(group => new
                {
                    GroupId = group.Id,
                    GroupName = group.Name,
                    CompletionRate = group.StudentsGroups.Any() ? (double)group.StudentsGroups.Count(sg => sg.Student.Results.Any()) / group.StudentsGroups.Count() * 100 : 0
                })
                .ToList();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            // Generate the statistical report based on the data
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Quiz Completion Rate by Group");

                // Add headers
                worksheet.Cells[1, 1].Value = "Группа";
                worksheet.Cells[1, 2].Value = "Коэффициент завершения (%)";

                worksheet.Cells[1, 1, 1, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                // Add data
                for (int i = 0; i < reportData.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = reportData[i].GroupName;
                    worksheet.Cells[i + 2, 2].Value = reportData[i].CompletionRate;

                    worksheet.Cells[i + 2, 1, i + 2, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                // Save the Excel file
                await package.SaveAsAsync(new FileInfo(@"D:\QuizCompletionRateByGroup.xlsx"));
            }
        }

        public async Task GenerateTopPerformingStudentsByQuizAsync()
        {
            var quizzes = await quizRepository.All().Include(q => q.Results).ThenInclude(x => x.Student).ToListAsync();

            var reportData = quizzes
                .Select(q => new
                {
                    QuizId = q.Id,
                    QuizName = q.Name,
                    TopPerformingStudents = q.Results.OrderByDescending(r => r.Points)
                        .Take(5)
                        .Select(r => new
                        {
                            StudentName = $"{r.Student.FirstName} {r.Student.LastName}",
                            Score = r.Points
                        })
                })
                .ToList();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            // Generate the statistical report based on the data
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Top Performing Students");

                // Add headers
                worksheet.Cells[1, 1].Value = "Викторина";
                worksheet.Cells[1, 2].Value = "Лучшие студенты";

                worksheet.Cells[1, 1, 1, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                // Add data
                for (int i = 0; i < reportData.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = reportData[i].QuizName;
                    worksheet.Cells[i + 2, 2].Value = string.Join(", ", reportData[i].TopPerformingStudents.Select(s => $"{s.StudentName} ({Math.Round(s.Score,2)})"));

                    worksheet.Cells[i + 2, 1, i + 2, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                // Save the Excel file
                await package.SaveAsAsync(new FileInfo(@"D:\TopPerformingStudents.xlsx"));
            }
        }

        public async Task GenerateQuizPerformanceReportByCategoryAsync()
        {
            var quizzes = await quizRepository.All().Include(q => q.Results).ToListAsync();

            var reportData = quizzes
                .GroupBy(q => q.CategoryId)
                .Select(group => new
                {
                    CategoryId = group.Key,
                    CategoryName = categoryRepository.All().FirstOrDefault(x => x.Id == group.Key)?.Name,
                    AverageScore = group.SelectMany(q => q.Results).DefaultIfEmpty().Average(r => r != null ? r.Points : 0)
                })
                .ToList();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Quiz Performance by Category");

                worksheet.Cells[1, 1].Value = "Категория";
                worksheet.Cells[1, 2].Value = "Средний балл";

                worksheet.Cells[1, 1, 1, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                for (int i = 0; i < reportData.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = reportData[i].CategoryName;
                    worksheet.Cells[i + 2, 2].Value = reportData[i].AverageScore;

                    worksheet.Cells[i + 2, 1, i + 2, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                var byteArray = await package.GetAsByteArrayAsync();
                await File.WriteAllBytesAsync(@"D:\QuizPerformanceByCategory.xlsx", byteArray);
            }
        }

        public async Task GenerateGroupPerformanceReportAsync()
        {
            var groups = groupRepository.All()
                .Include(g => g.StudentsGroups)
                .ThenInclude(sg => sg.Student);

            var reportData = await groups
                .Select(group => new
                {
                    GroupId = group.Id,
                    GroupName = group.Name,
                    AverageScore = repository.All()
                        .Where(r => group.StudentsGroups.Any(sg => sg.GroupId == group.Id))
                        .Average(r => r.Points)
                })
                .ToListAsync();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Group Performance");

                worksheet.Cells[1, 1].Value = "Группа";
                worksheet.Cells[1, 2].Value = "Средний балл";

                worksheet.Cells[1, 1, 1, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                for (int i = 0; i < reportData.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = reportData[i].GroupName;
                    worksheet.Cells[i + 2, 2].Value = reportData[i].AverageScore;

                    worksheet.Cells[i + 2, 1, i + 2, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                var byteArray = await package.GetAsByteArrayAsync();
                await File.WriteAllBytesAsync(@"D:\GroupPerformance.xlsx", byteArray);
            }
        }

        public async Task GenerateEventParticipationReportAsync()
        {
            var events = await eventRepository.All()
                .Include(e => e.EventsGroups)
                .ThenInclude(eg => eg.Group)
                .ToListAsync();

            var reportData = events
                .Select(e => new
                {
                    EventId = e.Id,
                    EventName = e.Name,
                    ParticipantCount = e.EventsGroups.Select(eg => eg.GroupId).Distinct().Count()
                })
                .ToList();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Event Participation");

                worksheet.Cells[1, 1].Value = "Событие";
                worksheet.Cells[1, 2].Value = "Количество участников";

                worksheet.Cells[1, 1, 1, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                for (int i = 0; i < reportData.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = reportData[i].EventName;
                    worksheet.Cells[i + 2, 2].Value = reportData[i].ParticipantCount;

                    worksheet.Cells[i + 2, 1, i + 2, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                worksheet.Cells.AutoFitColumns();

                var byteArray = await package.GetAsByteArrayAsync();
                await File.WriteAllBytesAsync(@"D:\EventParticipation.xlsx", byteArray);
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
                worksheet.Cells[1, 1, 1, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                // Populate the data rows
                for (var i = 0; i < students.Count; i++)
                {
                    var student = students[i];

                    worksheet.Cells[i + 2, 1].Value = student.Id;
                    worksheet.Cells[i + 2, 2].Value = student.FullName;
                    worksheet.Cells[i + 2, 3].Value = student.Email;

                    // Set center alignment for data cells
                    worksheet.Cells[i + 2, 1, i + 2, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
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