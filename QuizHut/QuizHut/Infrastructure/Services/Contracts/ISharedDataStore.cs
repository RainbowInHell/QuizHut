namespace QuizHut.Infrastructure.Services.Contracts
{
    using QuizHut.Infrastructure.EntityViewModels.Categories;
    using QuizHut.Infrastructure.EntityViewModels.Events;
    using QuizHut.Infrastructure.EntityViewModels.Groups;
    using QuizHut.Infrastructure.EntityViewModels.Quizzes;

    public interface ISharedDataStore
    {
        GroupListViewModel? SelectedGroup { get; set; }

        CategoryViewModel? SelectedCategory { get; set; }

        EventListViewModel? SelectedEvent { get; set; }

        QuizListViewModel? SelectedQuiz { get; set; }

        string SelectedQuestionId { get; set; }

        string SelectedAnswerId { get; set; }
    }
}