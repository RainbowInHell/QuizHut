namespace QuizHut.Infrastructure.Services.Contracts
{
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;

    using QuizHut.Infrastructure.EntityViewModels;
    using QuizHut.Infrastructure.EntityViewModels.Categories;
    using QuizHut.Infrastructure.EntityViewModels.Events;
    using QuizHut.Infrastructure.EntityViewModels.Groups;
    using QuizHut.Infrastructure.EntityViewModels.Quizzes;
    using QuizHut.Infrastructure.EntityViewModels.Results;

    public interface IExporter
    {
        Task GenerateStudentPerformanceDistributionByCategoryAsync();

        Task GenerateTimeSpentOnQuizzesByStudentAsync();

        Task GenerateQuizCompletionRateByGroupAsync();

        Task GenerateTopPerformingStudentsByQuizAsync();

        Task GenerateQuizPerformanceReportByCategoryAsync();

        Task GenerateGroupPerformanceReportAsync();

        Task GenerateEventParticipationReportAsync();

        Task GenerateComplexResultsExcelReportAsync();

        Task GenerateExcelReportAsync(ObservableCollection<StudentViewModel> students);

        Task GenerateExcelReportAsync(ObservableCollection<QuizListViewModel> quizzes);

        Task GenerateExcelReportAsync(ObservableCollection<CategoryViewModel> categories);

        Task GenerateExcelReportAsync(ObservableCollection<GroupListViewModel> groups);

        Task GenerateExcelReportAsync(ObservableCollection<StudentResultViewModel> studentResults, string studentName);

        Task GenerateExcelReportAsync(ObservableCollection<StudentActiveEventViewModel> studentActiveEvents);

        Task GenerateExcelReportAsync(ObservableCollection<StudentPendingEventViewModel> studentPendingEvents);

        Task GenerateExcelReportAsync(ObservableCollection<StudentEndedEventViewModel> studentEndedEvents);
    }
}