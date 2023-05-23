namespace QuizHut.Infrastructure.Services.Contracts
{
    using QuizHut.Infrastructure.EntityViewModels.Answers;
    using QuizHut.Infrastructure.EntityViewModels.Categories;
    using QuizHut.Infrastructure.EntityViewModels.Events;
    using QuizHut.Infrastructure.EntityViewModels.Groups;
    using QuizHut.Infrastructure.EntityViewModels.Questions;
    using QuizHut.Infrastructure.EntityViewModels.Quizzes;

    public interface ISharedDataStore
    {
        GroupListViewModel? SelectedGroup { get; set; }

        CategoryViewModel? SelectedCategory { get; set; }

        EventListViewModel? SelectedEvent { get; set; }

        QuizListViewModel? SelectedQuiz { get; set; }

        QuestionViewModel? SelectedQuestion { get; set; }

        AnswerViewModel? SelectedAnswer { get; set; }

        AttemptedQuizViewModel? QuizToPass { get; set; }

        AttemptedQuizQuestionViewModel? CurrentQuestion { get; set; }

        string CurrentResultId { get; set; }
    }
}