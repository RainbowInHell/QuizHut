namespace QuizHut.Infrastructure.Services
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Drawing;
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
    using QuizHut.Infrastructure.EntityViewModels.Results;
    using QuizHut.Infrastructure.Services.Contracts;

    public class Exporter : IExporter
    {
        private readonly IRepository<ApplicationUser> studentRepository;

        private readonly IRepository<Category> categoryRepository;

        private readonly IRepository<Event> eventRepository;

        private readonly IRepository<Group> groupRepository;

        private readonly IRepository<Quiz> quizRepository;

        private readonly IRepository<Result> resultRepository;

        public Exporter(
            IRepository<Result> resultRepository, 
            IRepository<Event> eventRepository, 
            IRepository<Quiz> quizRepository,
            IRepository<Category> categoryRepository,
            IRepository<Group> groupRepository,
            IRepository<ApplicationUser> studentRepository)
        {
            this.resultRepository = resultRepository;
            this.eventRepository = eventRepository;
            this.quizRepository = quizRepository;
            this.categoryRepository = categoryRepository;
            this.groupRepository = groupRepository;
            this.studentRepository = studentRepository;
        }

        //public async Task GenerateComplexResultsExcelReportAsync()
        //{
        //    var resultsWithEventsAndQuizzes = await resultRepository.All()
        //        .Include(r => r.Student)
        //        .Include(r => r.Quiz)
        //            .ThenInclude(q => q.Event)
        //                .ThenInclude(e => e.EventsGroups)
        //                    .ThenInclude(eg => eg.Group)
        //        .ToListAsync();

        //    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        //    using (var package = new ExcelPackage())
        //    {
        //        var worksheet = package.Workbook.Worksheets.Add("Better Report");

        //        worksheet.Cells[1, 1].Value = "Событие";
        //        worksheet.Cells[1, 2].Value = "Группа";
        //        worksheet.Cells[1, 3].Value = "Участник";
        //        worksheet.Cells[1, 4].Value = "Баллы";
        //        worksheet.Cells[1, 5].Value = "Максимально баллов";
        //        worksheet.Cells[1, 6].Value = "Зтраченное время";
        //        worksheet.Cells[1, 7].Value = "Дата";
        //        worksheet.Cells[1, 8].Value = "Викторина";

        //        ApplyReportStyling(worksheet, 1, 8);

        //        var groupedResults = resultsWithEventsAndQuizzes
        //            .GroupBy(r => r.Quiz?.Event?.Name)
        //            .ToList();

        //        int rowIndex = 2;

        //        foreach (var group in groupedResults)
        //        {
        //            string eventName = group.Key;

        //            // Add a new row for the event header
        //            worksheet.Cells[rowIndex, 1].Value = eventName;
        //            worksheet.Cells[rowIndex, 1, rowIndex, 8].Merge = true;
        //            worksheet.Cells[rowIndex, 1, rowIndex, 8].Style.Font.Bold = true;
        //            worksheet.Cells[rowIndex, 1, rowIndex, 8].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

        //            foreach (var result in group)
        //            {
        //                var groupName = result.Quiz?.Event?.EventsGroups?.FirstOrDefault()?.Group?.Name;
        //                var studentName = $"{result.Student?.FirstName} {result.Student?.LastName}";
        //                var points = result.Points;
        //                var maxPoints = result.MaxPoints;
        //                var timeSpent = result.TimeSpent.ToString(@"hh\:mm\:ss");
        //                var activationDate = result.Quiz?.Event?.ActivationDateAndTime.ToShortDateString();
        //                var quizName = result.Quiz?.Name;

        //                worksheet.Cells[rowIndex, 1].Value = eventName;
        //                worksheet.Cells[rowIndex, 2].Value = groupName;
        //                worksheet.Cells[rowIndex, 3].Value = studentName;
        //                worksheet.Cells[rowIndex, 4].Value = points;
        //                worksheet.Cells[rowIndex, 5].Value = maxPoints;
        //                worksheet.Cells[rowIndex, 6].Value = timeSpent;
        //                worksheet.Cells[rowIndex, 7].Value = activationDate;
        //                worksheet.Cells[rowIndex, 8].Value = quizName;

        //                ApplyReportStyling(worksheet, rowIndex, 8);

        //                rowIndex++;
        //            }
        //        }

        //        worksheet.Cells.AutoFitColumns();

        //        var byteArray = await package.GetAsByteArrayAsync();
        //        await File.WriteAllBytesAsync(@"D:\ComplexResultsReport.xlsx", byteArray);
        //    }
        //}
        public async Task GenerateComplexResultsExcelReportAsync()
        {
            var resultsWithEventsAndQuizzes = await resultRepository.All()
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
                worksheet.Cells[1, 2].Value = "Группа";
                worksheet.Cells[1, 3].Value = "Участник";
                worksheet.Cells[1, 4].Value = "Баллы";
                worksheet.Cells[1, 5].Value = "Максимально баллов";
                worksheet.Cells[1, 6].Value = "Зтраченное время";
                worksheet.Cells[1, 7].Value = "Дата";
                worksheet.Cells[1, 8].Value = "Викторина";

                worksheet.Cells[1, 1, 1, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[1, 1, 1, 8].Style.Font.Bold = true;
                worksheet.Cells[1, 1, 1, 8].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                var groupedResults = resultsWithEventsAndQuizzes
                    .GroupBy(r => r.Quiz?.Event?.Name)
                    .ToList();

                int rowIndex = 2;

                foreach (var group in groupedResults)
                {
                    string eventName = group.Key;

                    // Add a new row for the event header
                    worksheet.Cells[rowIndex, 1].Value = eventName;
                    worksheet.Cells[rowIndex, 1, rowIndex, 8].Merge = true;
                    worksheet.Cells[rowIndex, 1, rowIndex, 8].Style.Font.Bold = true;
                    worksheet.Cells[rowIndex, 1, rowIndex, 8].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                    foreach (var result in group)
                    {
                        var groupName = result.Quiz?.Event?.EventsGroups?.FirstOrDefault()?.Group?.Name;
                        var studentName = $"{result.Student?.FirstName} {result.Student?.LastName}";
                        var points = result.Points;
                        var maxPoints = result.MaxPoints;
                        var timeSpent = result.TimeSpent.ToString(@"hh\:mm\:ss");
                        var activationDate = result.Quiz?.Event?.ActivationDateAndTime.ToShortDateString();
                        var quizName = result.Quiz?.Name;

                        worksheet.Cells[rowIndex, 1].Value = eventName;
                        worksheet.Cells[rowIndex, 2].Value = groupName;
                        worksheet.Cells[rowIndex, 3].Value = studentName;
                        worksheet.Cells[rowIndex, 4].Value = points;
                        worksheet.Cells[rowIndex, 5].Value = maxPoints;
                        worksheet.Cells[rowIndex, 6].Value = timeSpent;
                        worksheet.Cells[rowIndex, 7].Value = activationDate;
                        worksheet.Cells[rowIndex, 8].Value = quizName;

                        worksheet.Cells[rowIndex, 1, rowIndex, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells[rowIndex, 1, rowIndex, 8].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        rowIndex++;
                    }
                }

                worksheet.Cells.AutoFitColumns();

                await package.SaveAsAsync(new FileInfo(@"D:\ComplexResultsReport.xlsx"));
            }
        }

        public async Task GenerateStudentPerformanceDistributionByCategoryAsync()
        {
            var quizzes = await quizRepository.All().Include(q => q.Results).ToListAsync();

            var reportData = quizzes
                .SelectMany(q => q.Results.Select(r => new
                {
                    CategoryId = q.CategoryId,
                    CategoryName = categoryRepository.All().FirstOrDefault(x => x.Id == q.CategoryId)?.Name,
                    StudentScore = r.Points
                }))
                .GroupBy(r => r.CategoryId)
                .Select(group => new
                {
                    CategoryId = group.Key,
                    CategoryName = group.FirstOrDefault()?.CategoryName,
                    ScoreDistribution = group.Select(r => Math.Round(r.StudentScore, 2))
                })
                .ToList();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Student Performance Distribution");

                // Add headers
                worksheet.Cells[1, 1].Value = "Категория";
                worksheet.Cells[1, 2].Value = "Распределение баллов";

                ApplyReportStyling(worksheet, reportData.Count, 2);

                // Add data
                for (int i = 0; i < reportData.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = reportData[i].CategoryName;
                    worksheet.Cells[i + 2, 2].Value = string.Join(", ", reportData[i].ScoreDistribution);

                    worksheet.Cells[i + 2, 1, i + 2, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                var byteArray = await package.GetAsByteArrayAsync();
                await File.WriteAllBytesAsync(@"D:\StudentPerformanceDistribution.xlsx", byteArray);
            }
        }

        public async Task GenerateTimeSpentOnQuizzesByStudentAsync()
        {
            var students = await studentRepository.All().Include(s => s.Results).ToListAsync();

            var reportData = students
                .Select(s => new
                {
                    StudentName = $"{s.FirstName} {s.LastName}",
                    TotalTimeSpent = s.Results.Sum(r => r.TimeSpent.TotalMinutes)
                })
                .ToList();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Time Spent on Quizzes by Student");

                // Add headers
                worksheet.Cells[1, 1].Value = "Участник";
                worksheet.Cells[1, 2].Value = "Всего потрачено времени (минут)";

                ApplyReportStyling(worksheet, reportData.Count, 2);

                // Add data
                for (int i = 0; i < reportData.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = reportData[i].StudentName;
                    worksheet.Cells[i + 2, 2].Value = reportData[i].TotalTimeSpent;

                    worksheet.Cells[i + 2, 1, i + 2, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                var byteArray = await package.GetAsByteArrayAsync();
                await File.WriteAllBytesAsync(@"D:\TimeSpentOnQuizzesByStudent.xlsx", byteArray);
            }
        }
        //
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

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Quiz Completion Rate by Group");

                // Add headers
                worksheet.Cells[1, 1].Value = "Группа";
                worksheet.Cells[1, 2].Value = "Коэффициент завершенности (%)";

                ApplyReportStyling(worksheet, reportData.Count, 2);

                // Add data
                for (int i = 0; i < reportData.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = reportData[i].GroupName;
                    worksheet.Cells[i + 2, 2].Value = reportData[i].CompletionRate;

                    worksheet.Cells[i + 2, 1, i + 2, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                var byteArray = await package.GetAsByteArrayAsync();
                await File.WriteAllBytesAsync(@"D:\QuizCompletionRateByGroup.xlsx", byteArray);
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

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Top Performing Students");

                // Add headers
                worksheet.Cells[1, 1].Value = "Викиторина";
                worksheet.Cells[1, 2].Value = "Лучшие участники";

                ApplyReportStyling(worksheet, reportData.Count, 2);

                // Add data
                for (int i = 0; i < reportData.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = reportData[i].QuizName;
                    worksheet.Cells[i + 2, 2].Value = string.Join(", ", reportData[i].TopPerformingStudents.Select(s => $"{s.StudentName} ({Math.Round(s.Score, 2)})"));

                    worksheet.Cells[i + 2, 1, i + 2, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                var byteArray = await package.GetAsByteArrayAsync();
                await File.WriteAllBytesAsync(@"D:\TopPerformingStudents.xlsx", byteArray);
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

                ApplyReportStyling(worksheet, reportData.Count, 2);

                for (int i = 0; i < reportData.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = reportData[i].CategoryName;
                    worksheet.Cells[i + 2, 2].Value = reportData[i].AverageScore;
                }

                var byteArray = await package.GetAsByteArrayAsync();
                await File.WriteAllBytesAsync(@"D:\QuizPerformanceByCategory.xlsx", byteArray);
            }
        }

        public async Task GenerateGroupPerformanceReportAsync()
        {
            var groups = groupRepository.All()
                .Include(g => g.StudentsGroups)
                .ThenInclude(sg => sg.Student)
                .Include(g => g.StudentsGroups)
                .ThenInclude(sg => sg.Student.Results)
                .ToList();

            var reportData = groups.Select(group => new
            {
                GroupId = group.Id,
                GroupName = group.Name,
                AverageScore = group.StudentsGroups
                    .SelectMany(sg => sg.Student.Results)
                    .Select(r => r.Points)
                    .DefaultIfEmpty(0) // Use 0 as the default value if there are no results
                    .Average()
            }).ToList();


            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Group Performance");

                ApplyReportStyling(worksheet, 1, 2);

                worksheet.Cells[1, 1].Value = "Группа";
                worksheet.Cells[1, 2].Value = "Средний балл";

                worksheet.Cells[1, 1, 1, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                for (int i = 0; i < reportData.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = reportData[i].GroupName;
                    worksheet.Cells[i + 2, 2].Value = reportData[i].AverageScore;

                    worksheet.Cells[i + 2, 1, i + 2, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                ApplyCellBorders(worksheet, 1, reportData.Count + 1, 1, 2);

                var byteArray = await package.GetAsByteArrayAsync();
                await File.WriteAllBytesAsync(@"D:\GroupPerformance.xlsx", byteArray);
            }
        }
        
        public async Task GenerateEventParticipationReportAsync()
        {
            var events = await eventRepository.All()
                .Include(e => e.EventsGroups)
                    .ThenInclude(eg => eg.Group.StudentsGroups)
                        .ThenInclude(sg => sg.Student)
                .ToListAsync();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Event Participation");

                worksheet.Cells[1, 1, 1, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[1, 1, 1, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                worksheet.Cells[1, 1, 1, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[1, 1, 1, 2].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                worksheet.Cells[1, 1, 1, 2].Style.Font.Bold = true;

                worksheet.Cells[1, 1].Value = "Событие";
                worksheet.Cells[1, 2].Value = "Количество участников";

                int rowIndex = 2;
                foreach (var ev in events)
                {
                    var participantCount = ev.EventsGroups
                        .SelectMany(eg => eg.Group.StudentsGroups)
                        .Select(sg => sg.Student)
                        .Distinct()
                        .Count();

                    worksheet.Cells[rowIndex, 1, rowIndex, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[rowIndex, 1, rowIndex, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    worksheet.Cells[rowIndex, 1].Value = ev.Name;
                    worksheet.Cells[rowIndex, 2].Value = participantCount;

                    rowIndex++;
                }

                worksheet.Cells.AutoFitColumns();

                worksheet.Cells[1, 1, rowIndex - 1, 2].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[1, 1, rowIndex - 1, 2].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[1, 1, rowIndex - 1, 2].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[1, 1, rowIndex - 1, 2].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                await package.SaveAsAsync(new FileInfo(@"D:\EventParticipation.xlsx"));
            }
        }

        public async Task GenerateExcelReportAsync(ObservableCollection<StudentViewModel> students)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Report");

                worksheet.Cells[1, 1, 1, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[1, 1, 1, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                worksheet.Cells[1, 1, 1, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[1, 1, 1, 2].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                worksheet.Cells[1, 1, 1, 2].Style.Font.Bold = true;

                worksheet.Cells[1, 1].Value = "Участник";
                worksheet.Cells[1, 2].Value = "Электронная почта";

                int rowIndex = 2;
                for (var i = 0; i < students.Count; i++)
                {
                    worksheet.Cells[rowIndex, 1, rowIndex, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[rowIndex, 1, rowIndex, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    var student = students[i];

                    worksheet.Cells[rowIndex, 1].Value = student.FullName;
                    worksheet.Cells[rowIndex, 2].Value = student.Email;

                    rowIndex++;
                }

                worksheet.Cells.AutoFitColumns();

                worksheet.Cells[1, 1, rowIndex - 1, 2].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[1, 1, rowIndex - 1, 2].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[1, 1, rowIndex - 1, 2].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[1, 1, rowIndex - 1, 2].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                await package.SaveAsAsync(new FileInfo(@"D:\StudentsReport.xlsx"));
            }
        }

        //Ну типо...
        public async Task GenerateExcelReportAsync(ObservableCollection<QuizListViewModel> quizzes)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Report");

                ApplyReportStyling(worksheet, 1, 7);

                worksheet.Cells[1, 1].Value = "Викторина";
                worksheet.Cells[1, 2].Value = "Количество вопросов";
                worksheet.Cells[1, 3].Value = "Категория";
                worksheet.Cells[1, 4].Value = "Описание";
                worksheet.Cells[1, 5].Value = "Пароль";
                worksheet.Cells[1, 6].Value = "Время на прохождение";
                worksheet.Cells[1, 7].Value = "Вопросы";

                int maxQuestionLength = 0;
                for (var i = 0; i < quizzes.Count; i++)
                {
                    var quiz = quizzes[i];

                    worksheet.Cells[i + 2, 1].Value = quiz.Name;
                    worksheet.Cells[i + 2, 2].Value = quiz.QuestionsCount;
                    worksheet.Cells[i + 2, 3].Value = quiz.CategoryName;
                    worksheet.Cells[i + 2, 4].Value = quiz.Description;
                    worksheet.Cells[i + 2, 5].Value = quiz.Password;
                    worksheet.Cells[i + 2, 6].Value = quiz.Timer;

                    if (quiz.Questions != null && quiz.Questions.Any())
                    {
                        worksheet.Cells[i + 2, 7].Value = string.Join("\n", quiz.Questions.Select(q => q.Text));

                        int maxQuestionLengthInQuiz = quiz.Questions.Max(q => q.Text.Length);
                        if (maxQuestionLengthInQuiz > maxQuestionLength)
                            maxQuestionLength = maxQuestionLengthInQuiz;
                    }

                    ApplyQuestionColumnStyling(worksheet, i + 2, 7);
                }

                ApplyCellBorders(worksheet, 1, quizzes.Count + 1, 1, 7);

                worksheet.Cells.AutoFitColumns();

                int questionColumnWidth = maxQuestionLength * 1;
                worksheet.Column(7).Width = questionColumnWidth;

                await package.SaveAsAsync(new FileInfo(@"D:\QuizzesReport.xlsx"));
            }
        }

        public async Task GenerateExcelReportAsync(ObservableCollection<CategoryViewModel> categories)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Report");

                worksheet.Cells[1, 1, 1, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[1, 1, 1, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                worksheet.Cells[1, 1, 1, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[1, 1, 1, 2].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                worksheet.Cells[1, 1, 1, 2].Style.Font.Bold = true;

                worksheet.Cells[1, 1].Value = "Категория";
                worksheet.Cells[1, 2].Value = "Количество викторин";

                int rowIndex = 2;
                for (var i = 0; i < categories.Count; i++)
                {
                    var category = categories[i];

                    worksheet.Cells[rowIndex, 1, rowIndex, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[rowIndex, 1, rowIndex, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    worksheet.Cells[rowIndex, 1].Value = category.Name;
                    worksheet.Cells[rowIndex, 2].Value = category.QuizzesCount;

                    rowIndex++;
                }

                worksheet.Cells.AutoFitColumns();

                worksheet.Cells[1, 1, rowIndex - 1, 2].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[1, 1, rowIndex - 1, 2].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[1, 1, rowIndex - 1, 2].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[1, 1, rowIndex - 1, 2].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                await package.SaveAsAsync(new FileInfo(@"D:\CategoriesReport.xlsx"));
            }
        }

        public async Task GenerateExcelReportAsync(ObservableCollection<GroupListViewModel> groups)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Report");

                worksheet.Cells[1, 1, 1, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[1, 1, 1, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                worksheet.Cells[1, 1, 1, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[1, 1, 1, 2].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                worksheet.Cells[1, 1, 1, 2].Style.Font.Bold = true;

                worksheet.Cells[1, 1].Value = "Группа";
                worksheet.Cells[1, 2].Value = "Участники";

                int rowIndex = 2;
                for (var i = 0; i < groups.Count; i++)
                {
                    worksheet.Cells[rowIndex, 1, rowIndex, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[rowIndex, 1, rowIndex, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    var group = groups[i];

                    var students = string.Join(", ", group.StudentsGroups.Select(sg => $"{sg.Student.FirstName} {sg.Student.LastName}"));

                    worksheet.Cells[rowIndex, 1].Value = group.Name;
                    worksheet.Cells[rowIndex, 2].Value = students;

                    rowIndex++;
                }

                worksheet.Cells.AutoFitColumns();

                worksheet.Cells[1, 1, rowIndex - 1, 2].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[1, 1, rowIndex - 1, 2].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[1, 1, rowIndex - 1, 2].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[1, 1, rowIndex - 1, 2].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                await package.SaveAsAsync(new FileInfo(@"D:\GroupsReport.xlsx"));
            }
        }

        public async Task GenerateExcelReportAsync(ObservableCollection<StudentResultViewModel> studentResults, string studentName)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add($"Результаты {studentName}");

                worksheet.Cells[1, 1].Value = "Участник:";
                worksheet.Cells[1, 2].Value = studentName;

                worksheet.Cells[2, 1].Value = "Дата экспорта:";
                worksheet.Cells[2, 2].Value = DateTime.Now.ToString("dd.MM.yyyy HH:mm");

                worksheet.Cells[4, 1, 4, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[4, 1, 4, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                worksheet.Cells[4, 1, 4, 5].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[4, 1, 4, 5].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                worksheet.Cells[4, 1, 4, 5].Style.Font.Bold = true;

                worksheet.Cells[4, 1].Value = "Событие";
                worksheet.Cells[4, 2].Value = "Викторина";
                worksheet.Cells[4, 3].Value = "Дата";
                worksheet.Cells[4, 4].Value = "Время прохождения";
                worksheet.Cells[4, 5].Value = "Баллы";

                int rowIndex = 5;
                foreach (var result in studentResults)
                {
                    worksheet.Cells[rowIndex, 1].Value = result.EventName;
                    worksheet.Cells[rowIndex, 2].Value = result.QuizName;
                    worksheet.Cells[rowIndex, 3].Value = result.Date;
                    worksheet.Cells[rowIndex, 4].Value = result.TimeSpent;
                    worksheet.Cells[rowIndex, 5].Value = result.Score;

                    rowIndex++;
                }

                worksheet.Cells.AutoFitColumns();

                worksheet.Cells[4, 1, rowIndex - 1, 5].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[4, 1, rowIndex - 1, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[4, 1, rowIndex - 1, 5].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[4, 1, rowIndex - 1, 5].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                string saveDate = DateTime.Now.ToString("dd_MM__HH_mm");
                await package.SaveAsAsync(new FileInfo($@"D:\{studentName}_OwnResultReport_{saveDate}.xlsx"));
            }
        }

        #region Helpers

        private void ApplyReportStyling(ExcelWorksheet worksheet, int rowCount, int columnCount)
        {
            worksheet.Cells[1, 1, 1, columnCount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[1, 1, 1, columnCount].Style.Font.Bold = true;

            worksheet.Cells[1, 1, rowCount, columnCount].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[1, 1, rowCount, columnCount].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[1, 1, rowCount, columnCount].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[1, 1, rowCount, columnCount].Style.Border.Right.Style = ExcelBorderStyle.Thin;

            worksheet.Cells[1, 1, 1, columnCount].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[1, 1, 1, columnCount].Style.Fill.BackgroundColor.SetColor(Color.LightGray);

            for (int i = 0; i < rowCount; i++)
            {
                worksheet.Cells[i + 2, 1, i + 2, columnCount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                worksheet.Cells[i + 2, 1, i + 2, columnCount].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[i + 2, 1, i + 2, columnCount].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[i + 2, 1, i + 2, columnCount].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[i + 2, 1, i + 2, columnCount].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            }

            worksheet.Cells.AutoFitColumns();
        }

        private void ApplyQuestionColumnStyling(ExcelWorksheet worksheet, int startRow, int column)
        {
            var columnCells = worksheet.Cells[startRow, column, worksheet.Dimension.End.Row, column];
            var columnStyle = worksheet.Column(column).Style;

            // Apply horizontal alignment to center
            columnStyle.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            // Apply vertical alignment to middle
            columnStyle.VerticalAlignment = ExcelVerticalAlignment.Center;

            // Apply wrap text
            columnStyle.WrapText = true;

            // Set row heights to fit the wrapped text
            columnCells.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            columnCells.Style.ShrinkToFit = true;

            // Adjust row heights
            for (int row = startRow; row <= worksheet.Dimension.End.Row; row++)
            {
                var cell = worksheet.Cells[row, column];
                cell.AutoFitColumns();
            }
        }

        #endregion

        void ApplyCellBorders(ExcelWorksheet worksheet, int startRow, int endRow, int startColumn, int endColumn)
        {
            using (var range = worksheet.Cells[startRow, startColumn, endRow, endColumn])
            {
                var borderStyle = ExcelBorderStyle.Thin;
                var borderColor = Color.Black;

                range.Style.Border.Top.Style = borderStyle;
                range.Style.Border.Top.Color.SetColor(borderColor);

                range.Style.Border.Bottom.Style = borderStyle;
                range.Style.Border.Bottom.Color.SetColor(borderColor);

                range.Style.Border.Left.Style = borderStyle;
                range.Style.Border.Left.Color.SetColor(borderColor);

                range.Style.Border.Right.Style = borderStyle;
                range.Style.Border.Right.Color.SetColor(borderColor);
            }
        }
    }
}