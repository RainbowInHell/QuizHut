namespace QuizHut.Infrastructure.Services.Contracts
{
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;

    using QuizHut.Infrastructure.EntityViewModels;
    using QuizHut.Infrastructure.EntityViewModels.Categories;
    using QuizHut.Infrastructure.EntityViewModels.Groups;
    using QuizHut.Infrastructure.EntityViewModels.Quizzes;

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
    }
}