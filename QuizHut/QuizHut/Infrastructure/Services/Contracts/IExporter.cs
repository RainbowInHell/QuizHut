namespace QuizHut.Infrastructure.Services.Contracts
{
    using System.Collections.ObjectModel;

    using QuizHut.Infrastructure.EntityViewModels;
    using QuizHut.Infrastructure.EntityViewModels.Categories;
    using QuizHut.Infrastructure.EntityViewModels.Groups;
    using QuizHut.Infrastructure.EntityViewModels.Quizzes;

    public interface IExporter
    {
        void GenerateExcelReport();

        void GenerateExcelReport(ObservableCollection<StudentViewModel> students);

        void GenerateExcelReport(ObservableCollection<QuizListViewModel> quizzes);

        void GenerateExcelReport(ObservableCollection<CategoryViewModel> categories);

        void GenerateExcelReport(ObservableCollection<GroupListViewModel> groups);
    }
}