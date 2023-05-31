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

        void GenerateExcelReport(ObservableCollection<StudentViewModel> students);

        void GenerateExcelReport(ObservableCollection<QuizListViewModel> quizzes);

        void GenerateExcelReport(ObservableCollection<CategoryViewModel> categories);

        void GenerateExcelReport(ObservableCollection<GroupListViewModel> groups);
    }
}