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
    using Microsoft.Win32;

    using OfficeOpenXml;
    using OfficeOpenXml.Style;
    
    using QuizHut.DLL.Entities;
    using QuizHut.DLL.Repositories.Contracts;
    using QuizHut.Infrastructure.EntityViewModels;
    using QuizHut.Infrastructure.EntityViewModels.Categories;
    using QuizHut.Infrastructure.EntityViewModels.Events;
    using QuizHut.Infrastructure.EntityViewModels.Groups;
    using QuizHut.Infrastructure.EntityViewModels.Quizzes;
    using QuizHut.Infrastructure.EntityViewModels.Results;
    using QuizHut.Infrastructure.Services.Contracts;

    public class Exporter : IExporter
    {
        private const string DEFAULT_FILE_NAME = "ExcelReport";

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

        public async Task<bool> SaveReportAsync(ExcelPackage package)
        {
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel Files (*.xlsx)|*.xlsx";
            saveFileDialog.DefaultExt = "xlsx";
            saveFileDialog.AddExtension = true;
            saveFileDialog.FileName = DEFAULT_FILE_NAME;

            if (saveFileDialog.ShowDialog() == true)
            {
                await package.SaveAsAsync(new FileInfo(saveFileDialog.FileName));

                return true;
            }

            return false;
        }

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

                worksheet.Cells[1, 1].Value = "Группа";
                worksheet.Cells[1, 2].Value = "Участник";
                worksheet.Cells[1, 3].Value = "Баллы";
                worksheet.Cells[1, 4].Value = "Максимально баллов";
                worksheet.Cells[1, 5].Value = "Затраченное время";
                worksheet.Cells[1, 6].Value = "Дата";
                worksheet.Cells[1, 7].Value = "Викторина";

                var headerRange = worksheet.Cells[1, 1, 1, 7];
                headerRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                headerRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                headerRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                headerRange.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                headerRange.Style.Font.Bold = true;

                var groupedResults = resultsWithEventsAndQuizzes
                    .GroupBy(r => r.Quiz?.Event?.Name) // Group by Event Name
                    .ToList();

                int rowIndex = 2;
                foreach (var group in groupedResults)
                {
                    string eventName = group.Key;

                    worksheet.Cells[rowIndex, 1].Value = eventName;
                    worksheet.Cells[rowIndex, 1, rowIndex, 7].Merge = true;
                    worksheet.Cells[rowIndex, 1, rowIndex, 7].Style.Font.Bold = true;
                    worksheet.Cells[rowIndex, 1, rowIndex, 7].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells[rowIndex, 1, rowIndex, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    rowIndex++; // Move to the next row for the group name

                    foreach (var result in group)
                    {
                        var groupName = result.Quiz?.Event?.EventsGroups?.FirstOrDefault()?.Group?.Name;
                        var studentName = $"{result.Student?.FirstName} {result.Student?.LastName}";
                        var points = result.Points;
                        var maxPoints = result.MaxPoints;
                        var timeSpent = result.TimeSpent.ToString(@"hh\:mm\:ss");
                        var activationDate = result.Quiz?.Event?.ActivationDateAndTime.ToShortDateString();
                        var quizName = result.Quiz?.Name;

                        worksheet.Cells[rowIndex, 1].Value = groupName; // Use groupName here to display the group name
                        worksheet.Cells[rowIndex, 2].Value = studentName;
                        worksheet.Cells[rowIndex, 3].Value = points;
                        worksheet.Cells[rowIndex, 4].Value = maxPoints;
                        worksheet.Cells[rowIndex, 5].Value = timeSpent;
                        worksheet.Cells[rowIndex, 6].Value = activationDate;
                        worksheet.Cells[rowIndex, 7].Value = quizName;

                        var rowRange = worksheet.Cells[rowIndex, 1, rowIndex, 7];
                        rowRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        rowIndex++; // Move to the next row for the next result
                    }
                }

                worksheet.Cells.AutoFitColumns();

                var dataRange = worksheet.Cells[1, 1, rowIndex - 1, 7];
                dataRange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                dataRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                dataRange.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                dataRange.Style.Border.Right.Style = ExcelBorderStyle.Thin;

                await SaveReportAsync(package);
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
                .Where(r => !string.IsNullOrEmpty(r.CategoryName)) // Filter out empty category names
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

                worksheet.Cells[1, 1, 1, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[1, 1, 1, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                worksheet.Cells[1, 1, 1, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[1, 1, 1, 2].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                worksheet.Cells[1, 1, 1, 2].Style.Font.Bold = true;

                worksheet.Cells[1, 1].Value = "Категория";
                worksheet.Cells[1, 2].Value = "Распределение баллов";

                int rowIndex = 2;
                for (int i = 0; i < reportData.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = reportData[i].CategoryName;
                    worksheet.Cells[i + 2, 2].Value = string.Join(", ", reportData[i].ScoreDistribution);

                    worksheet.Cells[rowIndex, 1, rowIndex, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[rowIndex, 1, rowIndex, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    rowIndex++;
                }

                worksheet.Cells.AutoFitColumns();

                worksheet.Cells[1, 1, rowIndex - 1, 2].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[1, 1, rowIndex - 1, 2].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[1, 1, rowIndex - 1, 2].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[1, 1, rowIndex - 1, 2].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                await SaveReportAsync(package);
            }
        }

        public async Task GenerateTimeSpentOnQuizzesByStudentAsync()
        {
            var students = await studentRepository.All().Include(s => s.Results).ToListAsync();

            var reportData = students
                .Select(s => new
                {
                    StudentName = $"{s.FirstName} {s.LastName}",
                    TotalTimeSpent = s.Results.Sum(r => Math.Round(r.TimeSpent.TotalMinutes,2))
                })
                .ToList();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Time Spent on Quizzes by Student");

                worksheet.Cells[1, 1, 1, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[1, 1, 1, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                worksheet.Cells[1, 1, 1, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[1, 1, 1, 2].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                worksheet.Cells[1, 1, 1, 2].Style.Font.Bold = true;

                worksheet.Cells[1, 1].Value = "Участник";
                worksheet.Cells[1, 2].Value = "Всего потрачено времени (минут)";

                int rowIndex = 2;
                for (int i = 0; i < reportData.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = reportData[i].StudentName;
                    worksheet.Cells[i + 2, 2].Value = reportData[i].TotalTimeSpent;

                    worksheet.Cells[rowIndex, 1, rowIndex, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[rowIndex, 1, rowIndex, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    rowIndex++;
                }
                
                worksheet.Cells.AutoFitColumns();

                worksheet.Cells[1, 1, rowIndex - 1, 2].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[1, 1, rowIndex - 1, 2].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[1, 1, rowIndex - 1, 2].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[1, 1, rowIndex - 1, 2].Style.Border.Right.Style = ExcelBorderStyle.Thin;
               
                await SaveReportAsync(package);
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

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Quiz Completion Rate by Group");

                worksheet.Cells[1, 1, 1, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[1, 1, 1, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                worksheet.Cells[1, 1, 1, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[1, 1, 1, 2].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                worksheet.Cells[1, 1, 1, 2].Style.Font.Bold = true;

                worksheet.Cells[1, 1].Value = "Группа";
                worksheet.Cells[1, 2].Value = "Коэффициент завершенности (%)";

                int rowIndex = 2;
                for (int i = 0; i < reportData.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = reportData[i].GroupName;
                    worksheet.Cells[i + 2, 2].Value = reportData[i].CompletionRate;

                    worksheet.Cells[rowIndex, 1, rowIndex, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[rowIndex, 1, rowIndex, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    rowIndex++;
                }

                worksheet.Cells.AutoFitColumns();

                worksheet.Cells[1, 1, rowIndex - 1, 2].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[1, 1, rowIndex - 1, 2].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[1, 1, rowIndex - 1, 2].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[1, 1, rowIndex - 1, 2].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                await SaveReportAsync(package);
            }
        }

        public async Task GenerateTopPerformingStudentsByQuizAsync()
        {
            var quizzes = await quizRepository.All()
                .Include(q => q.Results)
                .ThenInclude(x => x.Student)
                .ToListAsync();

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

                worksheet.Cells[1, 1, 1, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[1, 1, 1, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                worksheet.Cells[1, 1, 1, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[1, 1, 1, 2].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                worksheet.Cells[1, 1, 1, 2].Style.Font.Bold = true;

                worksheet.Cells[1, 1].Value = "Викторина";
                worksheet.Cells[1, 2].Value = "Лучшие участники";

                int rowIndex = 2;
                for (int i = 0; i < reportData.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = reportData[i].QuizName;
                    worksheet.Cells[i + 2, 2].Value = string.Join(", ", reportData[i].TopPerformingStudents.Select(s => $"{s.StudentName} ({Math.Round(s.Score, 2)})"));

                    worksheet.Cells[rowIndex, 1, rowIndex, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[rowIndex, 1, rowIndex, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    rowIndex++;
                }

                worksheet.Cells.AutoFitColumns();

                worksheet.Cells[1, 1, rowIndex - 1, 2].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[1, 1, rowIndex - 1, 2].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[1, 1, rowIndex - 1, 2].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[1, 1, rowIndex - 1, 2].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                await SaveReportAsync(package);
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
                    AverageScore = group.SelectMany(q => q.Results).DefaultIfEmpty().Average(r => r?.Points)
                })
                .Where(r => !string.IsNullOrEmpty(r.CategoryName)) // Filter out empty category names
                .ToList();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Quiz Performance by Category");

                worksheet.Cells[1, 1, 1, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[1, 1, 1, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                worksheet.Cells[1, 1, 1, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[1, 1, 1, 2].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                worksheet.Cells[1, 1, 1, 2].Style.Font.Bold = true;

                worksheet.Cells[1, 1].Value = "Категория";
                worksheet.Cells[1, 2].Value = "Средний балл";

                int rowIndex = 2;
                for (int i = 0; i < reportData.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = reportData[i].CategoryName;
                    worksheet.Cells[i + 2, 2].Value = reportData[i].AverageScore;

                    worksheet.Cells[rowIndex, 1, rowIndex, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[rowIndex, 1, rowIndex, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    rowIndex++;
                }

                worksheet.Cells.AutoFitColumns();

                worksheet.Cells[1, 1, rowIndex - 1, 2].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[1, 1, rowIndex - 1, 2].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[1, 1, rowIndex - 1, 2].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[1, 1, rowIndex - 1, 2].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                await SaveReportAsync(package);
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
                    .Select(r => Math.Round(r.Points,2))
                    .DefaultIfEmpty(0)
                    .Average()
            }).ToList();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Group Performance");

                worksheet.Cells[1, 1, 1, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[1, 1, 1, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                worksheet.Cells[1, 1, 1, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[1, 1, 1, 2].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                worksheet.Cells[1, 1, 1, 2].Style.Font.Bold = true;

                worksheet.Cells[1, 1].Value = "Группа";
                worksheet.Cells[1, 2].Value = "Средний балл";

                int rowIndex = 2;
                for (int i = 0; i < reportData.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = reportData[i].GroupName;
                    worksheet.Cells[i + 2, 2].Value = reportData[i].AverageScore;

                    worksheet.Cells[rowIndex, 1, rowIndex, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[rowIndex, 1, rowIndex, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    rowIndex++;
                }

                worksheet.Cells.AutoFitColumns();

                worksheet.Cells[1, 1, rowIndex - 1, 2].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[1, 1, rowIndex - 1, 2].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[1, 1, rowIndex - 1, 2].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[1, 1, rowIndex - 1, 2].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                await SaveReportAsync(package);
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

                await SaveReportAsync(package);
            }
        }

        public async Task GenerateExcelReportAsync(ObservableCollection<EventListViewModel> events)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Report");

                worksheet.Cells[1, 1, 1, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[1, 1, 1, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                worksheet.Cells[1, 1, 1, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[1, 1, 1, 4].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                worksheet.Cells[1, 1, 1, 4].Style.Font.Bold = true;

                worksheet.Cells[1, 1].Value = "Название";
                worksheet.Cells[1, 2].Value = "Начало";
                worksheet.Cells[1, 3].Value = "Продолжительность";
                worksheet.Cells[1, 4].Value = "Статус";

                int rowIndex = 2;
                for (var i = 0; i < events.Count; i++)
                {
                    worksheet.Cells[rowIndex, 1, rowIndex, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[rowIndex, 1, rowIndex, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    var student = events[i];

                    worksheet.Cells[rowIndex, 1].Value = student.Name;
                    worksheet.Cells[rowIndex, 2].Value = student.StartDate;
                    worksheet.Cells[rowIndex, 3].Value = student.Duration;
                    worksheet.Cells[rowIndex, 4].Value = student.StatusAsString;

                    rowIndex++;
                }

                worksheet.Cells.AutoFitColumns();

                worksheet.Cells[1, 1, rowIndex - 1, 4].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[1, 1, rowIndex - 1, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[1, 1, rowIndex - 1, 4].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[1, 1, rowIndex - 1, 4].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                await SaveReportAsync(package);
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

                await SaveReportAsync(package);
            }
        }

        public async Task GenerateExcelReportAsync(ObservableCollection<QuizListViewModel> quizzes)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Report");

                worksheet.Cells[1, 1, 1, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[1, 1, 1, 7].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                worksheet.Cells[1, 1, 1, 7].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[1, 1, 1, 7].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                worksheet.Cells[1, 1, 1, 7].Style.Font.Bold = true;

                worksheet.Cells[1, 1].Value = "Викторина";
                worksheet.Cells[1, 2].Value = "Количество вопросов";
                worksheet.Cells[1, 3].Value = "Категория";
                worksheet.Cells[1, 4].Value = "Описание";
                worksheet.Cells[1, 5].Value = "Пароль";
                worksheet.Cells[1, 6].Value = "Время на прохождение";
                worksheet.Cells[1, 7].Value = "Вопросы";

                int rowIndex = 2;
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

                    worksheet.Cells[rowIndex, 1, rowIndex, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[rowIndex, 1, rowIndex, 7].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    rowIndex++;
                }

                worksheet.Cells.AutoFitColumns();

                worksheet.Cells[1, 1, rowIndex - 1, 7].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[1, 1, rowIndex - 1, 7].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[1, 1, rowIndex - 1, 7].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[1, 1, rowIndex - 1, 7].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                //int questionColumnWidth = maxQuestionLength * 1;
                //worksheet.Column(7).Width = questionColumnWidth;

                await SaveReportAsync(package);
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

                await SaveReportAsync(package);
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

                await SaveReportAsync(package);
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

                await SaveReportAsync(package);
            }
        }

        public async Task GenerateExcelReportAsync(ObservableCollection<StudentActiveEventViewModel> studentActiveEvents)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Активные события");

                worksheet.Cells[1, 1, 1, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[1, 1, 1, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                worksheet.Cells[1, 1, 1, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[1, 1, 1, 2].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                worksheet.Cells[1, 1, 1, 2].Style.Font.Bold = true;

                worksheet.Cells[1, 1].Value = "Событие";
                worksheet.Cells[1, 2].Value = "Продолжительность";

                int rowIndex = 2;
                foreach (var studentActiveEvent in studentActiveEvents)
                {
                    worksheet.Cells[rowIndex, 1].Value = studentActiveEvent.Name;
                    worksheet.Cells[rowIndex, 2].Value = studentActiveEvent.Duration;

                    rowIndex++;
                }

                worksheet.Cells.AutoFitColumns();

                worksheet.Cells[1, 1, rowIndex - 1, 2].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[1, 1, rowIndex - 1, 2].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[1, 1, rowIndex - 1, 2].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[1, 1, rowIndex - 1, 2].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                await SaveReportAsync(package);
            }
        }

        public async Task GenerateExcelReportAsync(ObservableCollection<StudentPendingEventViewModel> studentPendingEvents)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Cобытия в ожидании");

                worksheet.Cells[1, 1, 1, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[1, 1, 1, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                worksheet.Cells[1, 1, 1, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[1, 1, 1, 3].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                worksheet.Cells[1, 1, 1, 3].Style.Font.Bold = true;

                worksheet.Cells[1, 1].Value = "Событие";
                worksheet.Cells[1, 2].Value = "Дата";
                worksheet.Cells[1, 3].Value = "Продолжительность";

                int rowIndex = 2;
                foreach (var studentPendingEvent in studentPendingEvents)
                {
                    worksheet.Cells[rowIndex, 1].Value = studentPendingEvent.Name;
                    worksheet.Cells[rowIndex, 2].Value = studentPendingEvent.Date;
                    worksheet.Cells[rowIndex, 2].Value = studentPendingEvent.Duration;

                    rowIndex++;
                }

                worksheet.Cells.AutoFitColumns();

                worksheet.Cells[1, 1, rowIndex - 1, 3].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[1, 1, rowIndex - 1, 3].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[1, 1, rowIndex - 1, 3].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[1, 1, rowIndex - 1, 3].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                await SaveReportAsync(package);
            }
        }

        public async Task GenerateExcelReportAsync(ObservableCollection<StudentEndedEventViewModel> studentEndedEvents)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Завершённые события");

                worksheet.Cells[1, 1, 1, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[1, 1, 1, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                worksheet.Cells[1, 1, 1, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[1, 1, 1, 2].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                worksheet.Cells[1, 1, 1, 2].Style.Font.Bold = true;

                worksheet.Cells[1, 1].Value = "Событие";
                worksheet.Cells[1, 2].Value = "Дата";

                int rowIndex = 2;
                foreach (var studentEndedEvent in studentEndedEvents)
                {
                    worksheet.Cells[rowIndex, 1].Value = studentEndedEvent.Name;
                    worksheet.Cells[rowIndex, 2].Value = studentEndedEvent.Date;

                    rowIndex++;
                }

                worksheet.Cells.AutoFitColumns();

                worksheet.Cells[1, 1, rowIndex - 1, 2].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[1, 1, rowIndex - 1, 2].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[1, 1, rowIndex - 1, 2].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[1, 1, rowIndex - 1, 2].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                await SaveReportAsync(package);
            }
        }
    }
}